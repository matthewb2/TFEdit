// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 4643 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

using TextFileEdit.Actions;
using TextFileEdit.Document;
using TextFileEdit.Gui.CompletionWindow;

namespace TextFileEdit
{
	public delegate bool KeyEventHandler(char ch);
	public delegate bool DialogKeyProcessor(Keys keyData);
	
	/// <summary>
	/// This class paints the textarea.
	/// </summary>
	[ToolboxItem(false)]
	public class TextArea : Control
	{
		bool hiddenMouseCursor = false;
		/// <summary>
		/// The position where the mouse cursor was when it was hidden. Sometimes the text editor gets MouseMove
		/// events when typing text even if the mouse is not moved.
		/// </summary>
		
		Point virtualTop        = new Point(0, 0);
		TextAreaControl         motherTextAreaControl;
		TextEditorControl       motherTextEditorControl;
		
		
		bool autoClearSelection = false;
		
		List<AbstractMargin> leftMargins = new List<AbstractMargin>();
		
		TextView      textView;
		GutterMargin  gutterMargin;
		
		SelectionManager selectionManager;
		Caret            caret;

		internal Point mousepos = new Point(0, 0);
		
		bool disposed;
		
				
		public TextEditorControl MotherTextEditorControl {
			get {
				return motherTextEditorControl;
			}
		}
		
		public TextAreaControl MotherTextAreaControl {
			get {
				return motherTextAreaControl;
			}
		}
		
		public SelectionManager SelectionManager {
			get {
				return selectionManager;
			}
		}
		
		public Caret Caret {
			get {
				return caret;
			}
		}
		
		public TextView TextView {
			get {
				return textView;
			}
		}
		
		public int MaxVScrollValue {
			get {
				return (Document.GetVisibleLine(Document.TotalNumberOfLines - 1) + 1 + TextView.VisibleLineCount * 2 / 3) * TextView.FontHeight;
			}
		}
		
		public Point VirtualTop {
			get {
				return virtualTop;
			}
			set {
				Point newVirtualTop = new Point(value.X, Math.Min(MaxVScrollValue, Math.Max(0, value.Y)));
				if (virtualTop != newVirtualTop) {
					virtualTop = newVirtualTop;
					
					Invalidate();
				}
				caret.UpdateCaretPosition();
			}
		}
		
		public bool AutoClearSelection {
			get {
				return autoClearSelection;
			}
			set {
				autoClearSelection = value;
			}
		}
		
		[Browsable(false)]
		public IDocument Document {
			get {
				return motherTextEditorControl.Document;
			}
		}
		
		
		public ITextEditorProperties TextEditorProperties {
			get {
				return motherTextEditorControl.TextEditorProperties;
			}
		}
		
		public TextArea(TextEditorControl motherTextEditorControl, TextAreaControl motherTextAreaControl)
		{
			this.motherTextAreaControl      = motherTextAreaControl;
			this.motherTextEditorControl    = motherTextEditorControl;
			
			caret            = new Caret(this);
			selectionManager = new SelectionManager(Document, this);
			
			ResizeRedraw = true;
			
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			
			textView = new TextView(this);
			
			gutterMargin = new GutterMargin(this);
			leftMargins.AddRange(new AbstractMargin[] { gutterMargin});
			OptionsChanged();
			
			new TextAreaMouseHandler(this).Attach();
			
			Document.TextContentChanged += new EventHandler(TextContentChanged);
			
		}
		
		public void UpdateMatchingBracket()
		{
			
		}
		
		void TextContentChanged(object sender, EventArgs e)
		{
			Caret.Position = new TextLocation(0, 0);
			
		}
		
		public void SetDesiredColumn()
		{
			Caret.DesiredColumn = TextView.GetDrawingXPos(Caret.Line, Caret.Column) + VirtualTop.X;
		}
		
		public void SetCaretToDesiredColumn()
		{
			FoldMarker dummy;
			Caret.Position = textView.GetLogicalColumn(Caret.Line, Caret.DesiredColumn + VirtualTop.X, out dummy);
		}
		
		public void OptionsChanged()
		{
			textView.OptionsChanged();
			caret.RecreateCaret();
			caret.UpdateCaretPosition();
			Refresh();
		}
		
		/// <summary>
		/// Shows the mouse cursor if it has been hidden.
		/// </summary>
		/// <param name="forceShow"><c>true</c> to always show the cursor or <c>false</c> to show it only if it has been moved since it was hidden.</param>
		internal void ShowHiddenCursor(bool forceShow)
		{
			
		}
				
		// external interface to the attached event
		internal void RaiseMouseMove(MouseEventArgs e)
		{
			OnMouseMove(e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
		
			this.Cursor = Cursors.Default;
		}
			
		
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			int currentXPos = 0;
			int currentYPos = 0;
			
			Graphics  g = e.Graphics;
			Rectangle clipRectangle = e.ClipRectangle;
			
			bool isFullRepaint = clipRectangle.X == 0 && clipRectangle.Y == 0
				&& clipRectangle.Width == this.Width && clipRectangle.Height == this.Height;
			//
			g.TextRenderingHint = this.TextEditorProperties.TextRenderingHint;
			
			foreach (AbstractMargin margin in leftMargins) {
				if (margin.IsVisible) {
					Rectangle marginRectangle = new Rectangle(currentXPos , currentYPos, margin.Size.Width, Height - currentYPos);
					if (marginRectangle != margin.DrawingPosition) {
						if (!isFullRepaint && !clipRectangle.Contains(marginRectangle)) {
							Invalidate(); // do a full repaint
						}
						margin.DrawingPosition = marginRectangle;
					}
					currentXPos += margin.DrawingPosition.Width;
					
					if (clipRectangle.IntersectsWith(marginRectangle)) {
						if (!marginRectangle.IsEmpty) {
							margin.Paint(g, marginRectangle);
						}
					}
					
				}
			}
			
			Rectangle textViewArea = new Rectangle(currentXPos, currentYPos, Width - currentXPos, Height - currentYPos);
			
			if (textViewArea != textView.DrawingPosition) {
				textView.DrawingPosition = textViewArea;
				BeginInvoke((MethodInvoker)caret.UpdateCaretPosition);
			}
			
			
			textView.Paint(g, textViewArea);
			base.OnPaint(e);
		}
		
		#region keyboard handling methods
		
		/// <summary>
		/// This method is called on each Keypress
		/// </summary>
		/// <returns>
		/// True, if the key is handled by this method and should NOT be
		/// inserted in the textarea.
		/// </returns>
		protected internal virtual bool HandleKeyPress(char ch)
		{
			if (KeyEventHandler != null) {
				return KeyEventHandler(ch);
			}
			return false;
		}
		
		
		internal bool IsReadOnly(int offset)
		{
			if (Document.ReadOnly) {
				return true;
			}
			else {
				return false;
			}
		}
		
		internal bool IsReadOnly(int offset, int length)
		{
			if (Document.ReadOnly) {
				return true;
			}
			if (TextEditorProperties.SupportReadOnlySegments) {
				return Document.MarkerStrategy.GetMarkers(offset, length).Exists(m=>m.IsReadOnly);
			} else {
				return false;
			}
		}
		
		public void SimulateKeyPress(char ch)
		{
			if (ch < ' ') {
				Console.WriteLine("space");
				return;
			}
			BeginUpdate();
			//
			try {
				// INSERT char
				if (!HandleKeyPress(ch)) {
					switch (Caret.CaretMode) {
						case CaretMode.InsertMode:
							InsertChar(ch);
							Console.WriteLine("insertChar called");
							break;
						default:
							Debug.Assert(false, "Unknown caret mode " + Caret.CaretMode);
							break;
					}
				}
				EndUpdate();
			} finally {
				
			}
		}
		
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			SimulateKeyPress(e.KeyChar);
			e.Handled = true;
		}
		
		/// <summary>
		/// This method executes a dialog key
		/// </summary>
		public bool ExecuteDialogKey(Keys keyData)
		{
			// try, if a dialog key processor was set to use this
			if (DoProcessDialogKey != null && DoProcessDialogKey(keyData)) {
				return true;
			}
			
			IEditAction action =  motherTextEditorControl.GetEditAction(keyData);
			
			if (action != null) {
				//BeginUpdate();
				try {

				} finally {
					//EndUpdate();
					Caret.UpdateCaretPosition();
				}
				return true;
			}
			return false;
		}
		
		protected override bool ProcessDialogKey(Keys keyData)
		{
			return ExecuteDialogKey(keyData) || base.ProcessDialogKey(keyData);
		}
		#endregion
		
		public void ScrollToCaret()
		{
			
		}
		
		public void BeginUpdate()
		{
			motherTextEditorControl.BeginUpdate();
		}
		
		public void EndUpdate()
		{
			motherTextEditorControl.EndUpdate();
		}
		
		public bool EnableCutOrPaste {
			get {
				if (motherTextAreaControl == null)
					return false;
				else
					return !IsReadOnly(Caret.Offset);
			}
		}
		
		string GenerateWhitespaceString(int length)
		{
			return new String(' ', length);
		}
		/// <remarks>
		/// Inserts a single character at the caret position
		/// </remarks>
		public void InsertChar(char ch)
		{
			bool updating = motherTextEditorControl.IsInUpdate;
			if (!updating) {
				BeginUpdate();
			}
			if (Char.IsWhiteSpace(ch) && ch != '\t' && ch != '\n') {
				ch = ' ';
			}
			LineSegment caretLine = Document.GetLineSegment(Caret.Line);
			int offset = Caret.Offset;
			int dc = Caret.Column;
			if (caretLine.Length < dc && ch != '\n') {
				Document.Insert(offset, GenerateWhitespaceString(dc - caretLine.Length) + ch);
			} else {
				Document.Insert(offset, ch.ToString());
			}
			Document.UndoStack.EndUndoGroup();
			++Caret.Column;
			
			if (!updating) {
				EndUpdate();
				UpdateLineToEnd(Caret.Line, Caret.Column);
			}
			
		}
		
		/// <remarks>
		/// Inserts a whole string at the caret position
		/// </remarks>
		public void InsertString(string str)
		{
			bool updating = motherTextEditorControl.IsInUpdate;
			if (!updating) {
				BeginUpdate();
			}
			try {
								
				int oldOffset = Document.PositionToOffset(Caret.Position);
				int oldLine   = Caret.Line;
				LineSegment caretLine = Document.GetLineSegment(Caret.Line);
				if (caretLine.Length < Caret.Column) {
					int whiteSpaceLength = Caret.Column - caretLine.Length;
					Document.Insert(oldOffset, GenerateWhitespaceString(whiteSpaceLength) + str);
					Caret.Position = Document.OffsetToPosition(oldOffset + str.Length + whiteSpaceLength);
				} else {
					Document.Insert(oldOffset, str);
					Caret.Position = Document.OffsetToPosition(oldOffset + str.Length);
				}
				
				UpdateLineToEnd(Caret.Line, Caret.Column);
			} finally {
				if (!updating) {
					EndUpdate();
				}
			}
		}
		
		/// <remarks>
		/// Replaces a char at the caret position
		/// </remarks>
		public void ReplaceChar(char ch)
		{
			bool updating = motherTextEditorControl.IsInUpdate;
			if (!updating) {
				BeginUpdate();
			}
			int lineNr   = Caret.Line;
			LineSegment  line = Document.GetLineSegment(lineNr);
			int offset = Document.PositionToOffset(Caret.Position);
			if (offset < line.Offset + line.Length) {
				Document.Replace(offset, 1, ch.ToString());
			} else {
				Document.Insert(offset, ch.ToString());
			}
			if (!updating) {
				EndUpdate();
				UpdateLineToEnd(lineNr, Caret.Column);
			}
			++Caret.Column;

		}
		
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing) {
				if (!disposed) {
					disposed = true;
					if (caret != null) {
						caret.Dispose();
					}
					if (selectionManager != null) {
						selectionManager.Dispose();
					}
					Document.TextContentChanged -= new EventHandler(TextContentChanged);
				
					motherTextAreaControl = null;
					motherTextEditorControl = null;
					
					textView.Dispose();
				}
			}
		}
		
		#region UPDATE Commands
		internal void UpdateLine(int line)
		{
			UpdateLines(0, line, line);
		}
		
		internal void UpdateLines(int lineBegin, int lineEnd)
		{
			UpdateLines(0, lineBegin, lineEnd);
		}
		
		internal void UpdateToEnd(int lineBegin)
		{	
			lineBegin = Document.GetVisibleLine(lineBegin);
			int y         = Math.Max(    0, (int)(lineBegin * textView.FontHeight));
			y = Math.Max(0, y - this.virtualTop.Y);
			Rectangle r = new Rectangle(0,y,Width, Height - y);
			Invalidate(r);
		}
		
		internal void UpdateLineToEnd(int lineNr, int xStart)
		{
			UpdateLines(xStart, lineNr, lineNr);
		}
		
		internal void UpdateLine(int line, int begin, int end)
		{
			UpdateLines(line, line);
		}
		int FirstPhysicalLine {
			get {
				return VirtualTop.Y / textView.FontHeight;
			}
		}
		internal void UpdateLines(int xPos, int lineBegin, int lineEnd)
		{
			InvalidateLines((int)(xPos * this.TextView.WideSpaceWidth), lineBegin, lineEnd);
		}
		
		void InvalidateLines(int xPos, int lineBegin, int lineEnd)
		{
			lineBegin     = Math.Max(Document.GetVisibleLine(lineBegin), FirstPhysicalLine);
			lineEnd       = Math.Min(Document.GetVisibleLine(lineEnd),   FirstPhysicalLine + textView.VisibleLineCount);
			int y         = Math.Max(    0, (int)(lineBegin  * textView.FontHeight));
			int height    = Math.Min(textView.DrawingPosition.Height, (int)((1 + lineEnd - lineBegin) * (textView.FontHeight + 1)));
			
			Rectangle r = new Rectangle(0, y - 1 - this.virtualTop.Y,
			                            Width, height + 3);
			
			Invalidate(r);
		}
		#endregion
		public event KeyEventHandler    KeyEventHandler;
		public event DialogKeyProcessor DoProcessDialogKey;
		
	}
}
