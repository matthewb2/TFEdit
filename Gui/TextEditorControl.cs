// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 4081 $</version>
// </file>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

using TextFileEdit.Document;

[assembly: CLSCompliant(true)]

namespace TextFileEdit
{
	/// <summary>
	/// This class is used for a basic text area control
	/// </summary>
    [ToolboxBitmap(typeof(TextEditorControl), "Resources.TextEditorControl.bmp")]
    [ToolboxItem(true)]
	public class TextEditorControl : TextEditorControlBase
	{
		protected Panel textAreaPanel     = new Panel();
		TextAreaControl primaryTextArea;
		//TextAreaControl secondaryTextArea = null;
		
		TextAreaControl activeTextAreaControl;
		
		public override TextAreaControl ActiveTextAreaControl {
			get {
				return activeTextAreaControl;
			}
		}
		
		protected void SetActiveTextAreaControl(TextAreaControl value)
		{
			if (activeTextAreaControl != value) {
				activeTextAreaControl = value;
				
				if (ActiveTextAreaControlChanged != null) {
					ActiveTextAreaControlChanged(this, EventArgs.Empty);
				}
			}
		}
		
		public event EventHandler ActiveTextAreaControlChanged;
		
		public TextEditorControl()
		{
			//
			SetStyle(ControlStyles.ContainerControl, true);
			
			//textAreaPanel.Dock = DockStyle.Fill;
			
			Document = (new DocumentFactory()).CreateDocument();
			Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy();
			
			primaryTextArea  = new TextAreaControl(this);
			
			activeTextAreaControl = primaryTextArea;
			
			primaryTextArea.TextArea.GotFocus += delegate {
				SetActiveTextAreaControl(primaryTextArea);
			};
			//
			primaryTextArea.Dock = DockStyle.Fill;
			textAreaPanel.Controls.Add(primaryTextArea);
			//
			textAreaPanel.Size = new Size(500, 400);
			textAreaPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
			textAreaPanel.Location = new Point(this.ClientSize.Width / 2 - textAreaPanel.Size.Width / 2, 10);
			
			Controls.Add(textAreaPanel);
			ResizeRedraw = true;
			
			Document.UpdateCommited += new EventHandler(CommitUpdateRequested);
			
			OptionsChanged();
			
			
			
			
		}
		
		
		public override void OptionsChanged()
		{
			primaryTextArea.OptionsChanged();
			
		}
		
		

		public void Undo()
		{
			if (Document.ReadOnly) {
				return;
			}
			if (Document.UndoStack.CanUndo) {
				BeginUpdate();
				Document.UndoStack.Undo();
				
				Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.WholeTextArea));
				this.primaryTextArea.TextArea.UpdateMatchingBracket();
				
				EndUpdate();
			}
		}
		
		public void Redo()
		{
			if (Document.ReadOnly) {
				return;
			}
			if (Document.UndoStack.CanRedo) {
				BeginUpdate();
				Document.UndoStack.Redo();
				
				Document.RequestUpdate(new TextAreaUpdate(TextAreaUpdateType.WholeTextArea));
				this.primaryTextArea.TextArea.UpdateMatchingBracket();
				
				EndUpdate();
			}
		}
		
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				Document.UndoStack.ClearAll();
				Document.UpdateCommited -= new EventHandler(CommitUpdateRequested);
				if (textAreaPanel != null) {
					
					if (primaryTextArea != null) {
						primaryTextArea.Dispose();
					}
					textAreaPanel.Dispose();
					textAreaPanel = null;
				}
			}
			base.Dispose(disposing);
		}
		
		#region Update Methods
		public override void EndUpdate()
		{
			base.EndUpdate();
			Document.CommitUpdate();
			if (!IsInUpdate) {
				ActiveTextAreaControl.Caret.OnEndUpdate();
			}
		}
		
		void CommitUpdateRequested(object sender, EventArgs e)
		{
			if (IsInUpdate) {
				return;
			}
			foreach (TextAreaUpdate update in Document.UpdateQueue) {
				switch (update.TextAreaUpdateType) {
					case TextAreaUpdateType.PositionToEnd:
						this.primaryTextArea.TextArea.UpdateToEnd(update.Position.Y);
						
						break;
					case TextAreaUpdateType.PositionToLineEnd:
					case TextAreaUpdateType.SingleLine:
						this.primaryTextArea.TextArea.UpdateLine(update.Position.Y);
						
						break;
					case TextAreaUpdateType.SinglePosition:
						this.primaryTextArea.TextArea.UpdateLine(update.Position.Y, update.Position.X, update.Position.X);
						
						break;
					case TextAreaUpdateType.LinesBetween:
						this.primaryTextArea.TextArea.UpdateLines(update.Position.X, update.Position.Y);
						
						break;
					case TextAreaUpdateType.WholeTextArea:
						this.primaryTextArea.TextArea.Invalidate();
						
						break;
				}
			}
			Document.UpdateQueue.Clear();

		}
		#endregion
		
		#region Printing routines
		int          curLineNr = 0;
		float        curTabIndent = 0;
		StringFormat printingStringFormat;
		
		void BeginPrint(object sender, PrintEventArgs ev)
		{
			curLineNr = 0;
			printingStringFormat = (StringFormat)System.Drawing.StringFormat.GenericTypographic.Clone();
			
			float[] tabStops = new float[100];
			for (int i = 0; i < tabStops.Length; ++i) {
				tabStops[i] = TabIndent * primaryTextArea.TextArea.TextView.WideSpaceWidth;
			}
			//
			printingStringFormat.SetTabStops(0, tabStops);
		}
		
		void Advance(ref float x, ref float y, float maxWidth, float size, float fontHeight)
		{
			if (x + size < maxWidth) {
				x += size;
			} else {
				x  = curTabIndent;
				y += fontHeight;
			}
		}
		
		float MeasurePrintingHeight(Graphics g, LineSegment line, float maxWidth)
		{
			float xPos = 0;
			float yPos = 0;
			float fontHeight = Font.GetHeight(g);

			curTabIndent = 0;
			FontContainer fontContainer = TextEditorProperties.FontContainer;
			foreach (TextWord word in line.Words) {
				switch (word.Type) {
					case TextWordType.Space:
						Advance(ref xPos, ref yPos, maxWidth, primaryTextArea.TextArea.TextView.SpaceWidth, fontHeight);
						break;
					case TextWordType.Word:
						SizeF drawingSize = g.MeasureString(word.Word, word.GetFont(fontContainer), new SizeF(maxWidth, fontHeight * 100), printingStringFormat);
						Advance(ref xPos, ref yPos, maxWidth, drawingSize.Width, fontHeight);
						break;
				}
			}
			return yPos + fontHeight;
		}
		
		void DrawLine(Graphics g, LineSegment line, float yPos, RectangleF margin)
		{
			float xPos = 0;
			float fontHeight = Font.GetHeight(g);

			curTabIndent = 0 ;
			
			FontContainer fontContainer = TextEditorProperties.FontContainer;
			foreach (TextWord word in line.Words) {
				switch (word.Type) {
					case TextWordType.Space:
						Advance(ref xPos, ref yPos, margin.Width, primaryTextArea.TextArea.TextView.SpaceWidth, fontHeight);
						break;
					case TextWordType.Word:

						g.DrawString(word.Word, word.GetFont(fontContainer), BrushRegistry.GetBrush(word.Color), xPos + margin.X, yPos);
						SizeF drawingSize = g.MeasureString(word.Word, word.GetFont(fontContainer), new SizeF(margin.Width, fontHeight * 100), printingStringFormat);
						Advance(ref xPos, ref yPos, margin.Width, drawingSize.Width, fontHeight);
						break;
				}
			}
		}
		
		void PrintPage(object sender, PrintPageEventArgs ev)
		{
			Graphics g = ev.Graphics;
			float yPos = ev.MarginBounds.Top;
			
			while (curLineNr < Document.TotalNumberOfLines) {
				LineSegment curLine  = Document.GetLineSegment(curLineNr);
				if (curLine.Words != null) {
					float drawingHeight = MeasurePrintingHeight(g, curLine, ev.MarginBounds.Width);
					if (drawingHeight + yPos > ev.MarginBounds.Bottom) {
						break;
					}
					
					DrawLine(g, curLine, yPos, ev.MarginBounds);
					yPos += drawingHeight;
				}
				++curLineNr;
			}
			
			// If more lines exist, print another page.
			ev.HasMorePages = curLineNr < Document.TotalNumberOfLines;
		}
		#endregion
	}
}
