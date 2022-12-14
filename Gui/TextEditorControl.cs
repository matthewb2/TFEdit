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
		
		TextAreaControl activeTextAreaControl;
		
		public override TextAreaControl ActiveTextAreaControl {
			get {
				return activeTextAreaControl;
			}
		}
		
		public TextEditorControl()
		{
			SetStyle(ControlStyles.ContainerControl, true);
			
			Document = (new DocumentFactory()).CreateDocument();
			Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy();
			
			primaryTextArea  = new TextAreaControl(this);
			
			activeTextAreaControl = primaryTextArea;
			
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
		
	}
}
