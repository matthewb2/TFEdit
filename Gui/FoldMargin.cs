// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Mike Krüger" email="mike@icsharpcode.net"/>
//     <version>$Revision: 3673 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using TextFileEdit.Document;

namespace TextFileEdit
{
	/// <summary>
	/// This class views the line numbers and folding markers.
	/// </summary>
	public class FoldMargin : AbstractMargin
	{
		int selectedFoldLine = -1;
		
		public override Size Size {
			get {
				return new Size((int)(textArea.TextView.FontHeight),
				                -1);
			}
		}
		
		public override bool IsVisible {
			get {
				return textArea.TextEditorProperties.EnableFolding;
			}
		}
		
		public FoldMargin(TextArea textArea) : base(textArea)
		{
		}
		
		public override void Paint(Graphics g, Rectangle rect)
		{
			if (rect.Width <= 0 || rect.Height <= 0) {
				return;
			}
			HighlightColor lineNumberPainterColor = textArea.Document.HighlightingStrategy.GetColorFor("LineNumbers");
			
			
			for (int y = 0; y < (DrawingPosition.Height + textArea.TextView.VisibleLineDrawingRemainder) / textArea.TextView.FontHeight + 1; ++y) {
				Rectangle markerRectangle = new Rectangle(DrawingPosition.X,
				                                          DrawingPosition.Top + y * textArea.TextView.FontHeight - textArea.TextView.VisibleLineDrawingRemainder,
				                                          DrawingPosition.Width,
				                                          textArea.TextView.FontHeight);
				
				if (rect.IntersectsWith(markerRectangle)) {
					// draw dotted separator line
					if (textArea.Document.TextEditorProperties.ShowLineNumbers) {
						g.FillRectangle(BrushRegistry.GetBrush(textArea.Enabled ? lineNumberPainterColor.BackgroundColor : SystemColors.InactiveBorder),
						                markerRectangle);
						
						g.DrawLine(BrushRegistry.GetDotPen(lineNumberPainterColor.Color),
						           base.drawingPosition.X,
						           markerRectangle.Y,
						           base.drawingPosition.X,
						           markerRectangle.Bottom);
					} else {
						g.FillRectangle(BrushRegistry.GetBrush(textArea.Enabled ? lineNumberPainterColor.BackgroundColor : SystemColors.InactiveBorder), markerRectangle);
					}
					
					int currentLine = textArea.Document.GetFirstLogicalLine(textArea.TextView.FirstPhysicalLine + y);
					if (currentLine < textArea.Document.TotalNumberOfLines) {
						PaintFoldMarker(g, currentLine, markerRectangle);
					}
				}
			}
		}
		
		bool SelectedFoldingFrom(List<FoldMarker> list)
		{
			if (list != null) {
				for (int i = 0; i < list.Count; ++i) {
					if (this.selectedFoldLine == list[i].StartLine) {
						return true;
					}
				}
			}
			return false;
		}
		
		void PaintFoldMarker(Graphics g, int lineNumber, Rectangle drawingRectangle)
		{
		}
		
		public override void HandleMouseMove(Point mousepos, MouseButtons mouseButtons)
		{
			bool  showFolding  = textArea.Document.TextEditorProperties.EnableFolding;
			int   physicalLine = + (int)((mousepos.Y + textArea.VirtualTop.Y) / textArea.TextView.FontHeight);
			int   realline     = textArea.Document.GetFirstLogicalLine(physicalLine);
			
			if (!showFolding || realline < 0 || realline + 1 >= textArea.Document.TotalNumberOfLines) {
				return;
			}
			
			List<FoldMarker> foldMarkers = textArea.Document.FoldingManager.GetFoldingsWithStart(realline);
			int oldSelection = selectedFoldLine;
			if (foldMarkers.Count > 0) {
				selectedFoldLine = realline;
			} else {
				selectedFoldLine = -1;
			}
			if (oldSelection != selectedFoldLine) {
				textArea.Refresh(this);
			}
		}
		
		public override void HandleMouseDown(Point mousepos, MouseButtons mouseButtons)
		{
			bool  showFolding  = textArea.Document.TextEditorProperties.EnableFolding;
			int   physicalLine = + (int)((mousepos.Y + textArea.VirtualTop.Y) / textArea.TextView.FontHeight);
			int   realline     = textArea.Document.GetFirstLogicalLine(physicalLine);
			
			// focus the textarea if the user clicks on the line number view
			textArea.Focus();
			
			if (!showFolding || realline < 0 || realline + 1 >= textArea.Document.TotalNumberOfLines) {
				return;
			}
			
			List<FoldMarker> foldMarkers = textArea.Document.FoldingManager.GetFoldingsWithStart(realline);
			foreach (FoldMarker fm in foldMarkers) {
				fm.IsFolded = !fm.IsFolded;
			}
			textArea.Document.FoldingManager.NotifyFoldingsChanged(EventArgs.Empty);
		}
		
		public override void HandleMouseLeave(EventArgs e)
		{
			if (selectedFoldLine != -1) {
				selectedFoldLine = -1;
				textArea.Refresh(this);
			}
		}
		
		#region Drawing functions
		void DrawFoldMarker(Graphics g, RectangleF rectangle, bool isOpened, bool isSelected)
		{
			HighlightColor foldMarkerColor = textArea.Document.HighlightingStrategy.GetColorFor("FoldMarker");
			HighlightColor foldLineColor   = textArea.Document.HighlightingStrategy.GetColorFor("FoldLine");
			HighlightColor selectedFoldLine = textArea.Document.HighlightingStrategy.GetColorFor("SelectedFoldLine");
			
			Rectangle intRect = new Rectangle((int)rectangle.X, (int)rectangle.Y, (int)rectangle.Width, (int)rectangle.Height);
			g.FillRectangle(BrushRegistry.GetBrush(foldMarkerColor.BackgroundColor), intRect);
			g.DrawRectangle(BrushRegistry.GetPen(isSelected ? selectedFoldLine.Color : foldLineColor.Color), intRect);
			
			int space  = (int)Math.Round(((double)rectangle.Height) / 8d) + 1;
			int mid    = intRect.Height / 2 + intRect.Height % 2;
			
			// draw minus
			g.DrawLine(BrushRegistry.GetPen(foldMarkerColor.Color),
			           rectangle.X + space,
			           rectangle.Y + mid,
			           rectangle.X + rectangle.Width - space,
			           rectangle.Y + mid);
			
			// draw plus
			if (!isOpened) {
				g.DrawLine(BrushRegistry.GetPen(foldMarkerColor.Color),
				           rectangle.X + mid,
				           rectangle.Y + space,
				           rectangle.X + mid,
				           rectangle.Y + rectangle.Height - space);
			}
		}
		#endregion
	}
}
