// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 4888 $</version>
// </file>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using TextFileEdit.Document;

namespace TextFileEdit
{
	/// <summary>
	/// This class paints the textarea.
	/// </summary>
	[ToolboxItem(false)]
	public class TextAreaControl : Panel
	{
		TextEditorControl motherTextEditorControl;
		TextArea   textArea;
		bool       disposed;
		
		public TextArea TextArea {
			get {
				return textArea;
			}
		}
		
		public SelectionManager SelectionManager {
			get {
				return textArea.SelectionManager;
			}
		}
		
		public Caret Caret {
			get {
				return textArea.Caret;
			}
		}
		
		[Browsable(false)]
		public IDocument Document {
			get {
				if (motherTextEditorControl != null)
					return motherTextEditorControl.Document;
				return null;
			}
		}
		
		public ITextEditorProperties TextEditorProperties {
			get {
				if (motherTextEditorControl != null)
					return motherTextEditorControl.TextEditorProperties;
				return null;
			}
		}
		
		public TextAreaControl(TextEditorControl motherTextEditorControl)
		{
			this.motherTextEditorControl = motherTextEditorControl;
			this.textArea                = new TextArea(motherTextEditorControl, this);
			Controls.Add(textArea);
			ResizeRedraw = true;
			
		}
		
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (!disposed) {
					disposed = true;
					motherTextEditorControl = null;
					
				}
			}
			base.Dispose(disposing);
		}
		
		void DocumentTextContentChanged(object sender, EventArgs e)
		{
			Caret.ValidateCaretPos();
		}
		
		protected override void OnResize(System.EventArgs e)
		{
			base.OnResize(e);
			ResizeTextArea();
		}
		
		public void ResizeTextArea()
		{
			int y = 0;
			int h = 0;
			
			textArea.Bounds = new Rectangle(0, y,
			                                Width - SystemInformation.HorizontalScrollBarArrowWidth,
			                                Height - SystemInformation.VerticalScrollBarArrowHeight - h);
			
		}
				
		int[] lineLengthCache;
		const int LineLengthCacheAdditionalSize = 100;
		
		public void AdjustScrollBars()
		{
		
			int firstLine = textArea.TextView.FirstVisibleLine;
			int lastLine = this.Document.GetFirstLogicalLine(textArea.TextView.FirstPhysicalLine + textArea.TextView.VisibleLineCount);
			if (lastLine >= this.Document.TotalNumberOfLines)
				lastLine = this.Document.TotalNumberOfLines - 1;
			
			if (lineLengthCache == null || lineLengthCache.Length <= lastLine) {
				lineLengthCache = new int[lastLine + LineLengthCacheAdditionalSize];
			}
			
			for (int lineNumber = firstLine; lineNumber <= lastLine; lineNumber++) {
				LineSegment lineSegment = this.Document.GetLineSegment(lineNumber);
				
			}
			
		}
		
		public void OptionsChanged()
		{
			textArea.OptionsChanged();
				
			
		}
		
		
				
	}
}
