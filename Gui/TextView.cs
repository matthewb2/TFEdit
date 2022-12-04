// <file>
//     <copyright see="prj:///doc/copyright.txt"/>
//     <license see="prj:///doc/license.txt"/>
//     <owner name="Daniel Grunwald" email="daniel@danielgrunwald.de"/>
//     <version>$Revision: 3756 $</version>
// </file>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using TextFileEdit.Document;

namespace TextFileEdit
{
	/// <summary>
	/// This class paints the textarea.
	/// </summary>
	public class TextView : AbstractMargin, IDisposable
	{
		int fontHeight;

		public void Dispose()
		{
			measureCache.Clear();
		}
		
		public int FirstPhysicalLine {
			get {
				return textArea.VirtualTop.Y / fontHeight;
			}
		}
		public int LineHeightRemainder {
			get {
				return textArea.VirtualTop.Y % fontHeight;
			}
		}
		/// <summary>Gets the first visible <b>logical</b> line.</summary>
		public int FirstVisibleLine {
			get {
				return textArea.Document.GetFirstLogicalLine(textArea.VirtualTop.Y / fontHeight);
			}
			set {
				if (FirstVisibleLine != value) {
					textArea.VirtualTop = new Point(textArea.VirtualTop.X, textArea.Document.GetVisibleLine(value) * fontHeight);
					
				}
			}
		}
		
		public int VisibleLineDrawingRemainder {
			get {
				return textArea.VirtualTop.Y % fontHeight;
			}
		}
		
		public int FontHeight {
			get {
				return fontHeight;
			}
		}
		
		public int VisibleLineCount {
			get {
				return 1 + DrawingPosition.Height / fontHeight;
			}
		}
		
		public TextView(TextArea textArea) : base(textArea)
		{
			base.Cursor = Cursors.IBeam;
			OptionsChanged();
			
		}
		
		static int GetFontHeight(Font font)
		{
			int height1 = TextRenderer.MeasureText("_", font).Height;
			//
			int height2 = (int)Math.Ceiling(font.GetHeight());
			return Math.Max(height1, height2) + 1;
		}
		
		int spaceWidth;
		
		/// <summary>
		/// Gets the width of a space character.
		/// This value can be quite small in some fonts - consider using WideSpaceWidth instead.
		/// </summary>
		public int SpaceWidth {
			get {
				return spaceWidth;
			}
		}
		
		int wideSpaceWidth;
		
		/// <summary>
		/// Gets the width of a 'wide space' (=one quarter of a tab, if tab is set to 4 spaces).
		/// On monospaced fonts, this is the same value as spaceWidth.
		/// </summary>
		public int WideSpaceWidth {
			get {
				return wideSpaceWidth;
			}
		}
		
		Font lastFont;
		
		public void OptionsChanged()
		{
			this.lastFont = TextEditorProperties.FontContainer.RegularFont;
			this.fontHeight = GetFontHeight(lastFont);
			this.spaceWidth = Math.Max(GetWidth(' ', lastFont), 1);
			this.wideSpaceWidth = Math.Max(spaceWidth, GetWidth('x', lastFont));
		}
		
		#region Paint functions
		public override void Paint(Graphics g, Rectangle rect)
		{
			if (rect.Width <= 0 || rect.Height <= 0) {
				return;
			}
			
			// Just to ensure that fontHeight and char widths are always correct...
			if (lastFont != TextEditorProperties.FontContainer.RegularFont) {
				OptionsChanged();
				textArea.Invalidate();
			}
			
			int horizontalDelta = textArea.VirtualTop.X;

			for (int y = 0; y < (DrawingPosition.Height + VisibleLineDrawingRemainder) / fontHeight + 1; ++y) {
				Rectangle lineRectangle = new Rectangle(DrawingPosition.X - horizontalDelta,
				                                        DrawingPosition.Top + y * fontHeight - VisibleLineDrawingRemainder,
				                                        DrawingPosition.Width + horizontalDelta,
				                                        fontHeight);
				
				int currentLine = textArea.Document.GetFirstLogicalLine(textArea.Document.GetVisibleLine(FirstVisibleLine) + y);
				PaintDocumentLine(g, currentLine, lineRectangle);
			}
			textArea.Caret.PaintCaret(g);
		}
		
		void PaintDocumentLine(Graphics g, int lineNumber, Rectangle lineRectangle)
		{
			
			Brush bgColorBrush    = new SolidBrush(Color.White);
			Brush backgroundBrush = textArea.Enabled ? bgColorBrush : SystemBrushes.InactiveBorder;
			
			if (lineNumber >= textArea.Document.TotalNumberOfLines) {
					g.FillRectangle(backgroundBrush, lineRectangle);
				return;
			}
			
			int physicalXPos = lineRectangle.X;
			
			physicalXPos = PaintLinePart(g, lineNumber, 0, textArea.Document.GetLineSegment(lineNumber).Length, lineRectangle, physicalXPos);
			
			if (lineNumber < textArea.Document.TotalNumberOfLines) {
				ColumnRange    selectionRange = textArea.SelectionManager.GetSelectionAtLine(lineNumber);
				LineSegment    currentLine    = textArea.Document.GetLineSegment(lineNumber);
								
				bool  selectionBeyondEOL = selectionRange.EndColumn > currentLine.Length || ColumnRange.WholeColumn.Equals(selectionRange);
				Brush fillBrush = selectionBeyondEOL && TextEditorProperties.AllowCaretBeyondEOL ? bgColorBrush : backgroundBrush;
				
				g.FillRectangle(fillBrush, new RectangleF(physicalXPos, lineRectangle.Y, lineRectangle.Width - physicalXPos + lineRectangle.X, lineRectangle.Height));
				//Console.WriteLine("line:" + lineRectangle.Y);
				
			}
		}
		
		int PaintLinePart(Graphics g, int lineNumber, int startColumn, int endColumn, Rectangle lineRectangle, int physicalXPos)
		{

			LineSegment currentLine    = textArea.Document.GetLineSegment(lineNumber);
			Console.WriteLine("current line: " + currentLine.ToString());
			if (currentLine.Words == null) {
				return physicalXPos;
			}
			int currentWordOffset = 0;
			TextWord currentWord;
			TextWord nextCurrentWord = null;
			FontContainer fontContainer = TextEditorProperties.FontContainer;
			for (int wordIdx = 0; wordIdx < currentLine.Words.Count; wordIdx++) {
				currentWord = currentLine.Words[wordIdx];
				if (currentWordOffset < startColumn) {
					currentWordOffset += currentWord.Length;
					continue;
				}
			repeatDrawCurrentWord:
				if (currentWordOffset >= endColumn || physicalXPos >= lineRectangle.Right) {
					break;
				}
				
				Color wordForeColor = currentWord.Color;
				
				// Create solid brush.
				SolidBrush wordBackBrush = new SolidBrush(Color.White);
				
				int wordWidth = DrawDocumentWord(g, currentWord.Word,
												 new Point(physicalXPos, lineRectangle.Y),
												 currentWord.GetFont(fontContainer),
												 wordForeColor,
												 wordBackBrush);
				//
				physicalXPos += wordWidth;
				
				currentWordOffset += currentWord.Length;
				if (nextCurrentWord != null) {
					currentWord = nextCurrentWord;
					nextCurrentWord = null;
					goto repeatDrawCurrentWord;
				}
			}
			return physicalXPos;
		}
		
		int DrawDocumentWord(Graphics g, string word, Point position, Font font, Color foreColor, Brush backBrush)
		{
			if (word == null || word.Length == 0) {
				return 0;
			}
			/*
			if (word.Length > MaximumWordLength) {
				Console.WriteLine("ddd");
				int width = 0;
				for (int i = 0; i < word.Length; i += MaximumWordLength) {
					Point pos = position;
					pos.X += width;
					if (i + MaximumWordLength < word.Length)
						width += DrawDocumentWord(g, word.Substring(i, MaximumWordLength), pos, font, foreColor, backBrush);
					else
						width += DrawDocumentWord(g, word.Substring(i, word.Length - i), pos, font, foreColor, backBrush);
				}
				return width;
			}
			*/
			int wordWidth = MeasureStringWidth(g, word, font);
			
			g.FillRectangle(backBrush, new RectangleF(position.X, position.Y, wordWidth + 1, FontHeight));
			
			DrawString(g, word,
			           font,
			           foreColor,
			           position.X,
			           position.Y);
			return wordWidth;
		}
		
		struct WordFontPair {
			string word;
			Font font;
			public WordFontPair(string word, Font font) {
				this.word = word;
				this.font = font;
			}
		
			
		}
		
		Dictionary<WordFontPair, int> measureCache = new Dictionary<WordFontPair, int>();
		
		// split words after 1000 characters. Fixes GDI+ crash on very longs words, for example
		// a 100 KB Base64-file without any line breaks.
		const int MaximumWordLength = 1000;
		const int MaximumCacheSize = 2000;
		
		int MeasureStringWidth(Graphics g, string word, Font font)
		{
			int width;
			if (word == null || word.Length == 0)
				return 0;
			/*
			if (word.Length > MaximumWordLength) {
				width = 0;
				for (int i = 0; i < word.Length; i += MaximumWordLength) {
					if (i + MaximumWordLength < word.Length)
						width += MeasureStringWidth(g, word.Substring(i, MaximumWordLength), font);
					else
						width += MeasureStringWidth(g, word.Substring(i, word.Length - i), font);
				}
				return width;
			}
			*/
			if (measureCache.TryGetValue(new WordFontPair(word, font), out width)) {
				return width;
			}
			if (measureCache.Count > MaximumCacheSize) {
				measureCache.Clear();
			}
			
			// Replaced GDI+ measurement with GDI measurement: faster and even more exact
			width = TextRenderer.MeasureText(g, word, font, new Size(short.MaxValue, short.MaxValue), textFormatFlags).Width;
			measureCache.Add(new WordFontPair(word, font), width);
			return width;
		}
		
		const TextFormatFlags textFormatFlags =
			TextFormatFlags.NoPadding | TextFormatFlags.NoPrefix | TextFormatFlags.PreserveGraphicsClipping;
		#endregion
		
		#region Conversion Functions
		Dictionary<Font, Dictionary<char, int>> fontBoundCharWidth = new Dictionary<Font, Dictionary<char, int>>();
		
		public int GetWidth(char ch, Font font)
		{
			if (!fontBoundCharWidth.ContainsKey(font)) {
				fontBoundCharWidth.Add(font, new Dictionary<char, int>());
			}
			if (!fontBoundCharWidth[font].ContainsKey(ch)) {
				using (Graphics g = textArea.CreateGraphics()) {
					return GetWidth(g, ch, font);
				}
			}
			return fontBoundCharWidth[font][ch];
		}
		
		public int GetWidth(Graphics g, char ch, Font font)
		{
			if (!fontBoundCharWidth.ContainsKey(font)) {
				fontBoundCharWidth.Add(font, new Dictionary<char, int>());
			}
			if (!fontBoundCharWidth[font].ContainsKey(ch)) {
				fontBoundCharWidth[font].Add(ch, MeasureStringWidth(g, ch.ToString(), font));
			}
			return fontBoundCharWidth[font][ch];
		}
		
		public int GetVisualColumn(int logicalLine, int logicalColumn)
		{
			int column = 0;
			using (Graphics g = textArea.CreateGraphics()) {
				CountColumns(ref column, 0, logicalColumn, logicalLine, g);
			}
			return column;
		}
		
		/// <summary>
		/// returns line/column for a visual point position
		/// </summary>
		public TextLocation GetLogicalPosition(Point mousePosition)
		{
			FoldMarker dummy;
			return GetLogicalColumn(GetLogicalLine(mousePosition.Y), mousePosition.X, out dummy);
		}
		
		/// <summary>
		/// returns line/column for a visual point position
		/// </summary>
		public TextLocation GetLogicalPosition(int visualPosX, int visualPosY)
		{
			FoldMarker dummy;
			return GetLogicalColumn(GetLogicalLine(visualPosY), visualPosX, out dummy);
		}
		
		/// <summary>
		/// returns line/column for a visual point position
		/// </summary>
		public FoldMarker GetFoldMarkerFromPosition(int visualPosX, int visualPosY)
		{
			FoldMarker foldMarker;
			GetLogicalColumn(GetLogicalLine(visualPosY), visualPosX, out foldMarker);
			return foldMarker;
		}
		
		/// <summary>
		/// returns logical line number for a visual point
		/// </summary>
		public int GetLogicalLine(int visualPosY)
		{
			int clickedVisualLine = Math.Max(0, (visualPosY + this.textArea.VirtualTop.Y) / fontHeight);
			return Document.GetFirstLogicalLine(clickedVisualLine);
		}
		
		internal TextLocation GetLogicalColumn(int lineNumber, int visualPosX, out FoldMarker inFoldMarker)
		{
			visualPosX += textArea.VirtualTop.X;
			
			inFoldMarker = null;
			if (lineNumber >= Document.TotalNumberOfLines) {
				return new TextLocation((int)(visualPosX / WideSpaceWidth), lineNumber);
			}
			if (visualPosX <= 0) {
				return new TextLocation(0, lineNumber);
			}
			
			int result=0;
			
			return new TextLocation(result, lineNumber);
		}
		
		
		float CountColumns(ref int column, int start, int end, int logicalLine, Graphics g)
		{
			if (start > end) throw new ArgumentException("start > end");
			if (start == end) return 0;
			//
			float drawingPos = 0;
			
			LineSegment currentLine = Document.GetLineSegment(logicalLine);
			List<TextWord> words = currentLine.Words;
			if (words == null) return 0;
			int wordCount = words.Count;
			int wordOffset = 0;
			FontContainer fontContainer = TextEditorProperties.FontContainer;
			for (int i = 0; i < wordCount; i++) {
				TextWord word = words[i];
				if (wordOffset >= end)
					break;
				if (wordOffset + word.Length >= start) {
					
					int wordStart = Math.Max(wordOffset, start);
					int wordLength = Math.Min(wordOffset + word.Length, end) - wordStart;
					string text = Document.GetText(currentLine.Offset + wordStart, wordLength);
					drawingPos += MeasureStringWidth(g, text, word.GetFont(fontContainer) ?? fontContainer.RegularFont);
				}
				wordOffset += word.Length;
			}
			for (int j = currentLine.Length; j < end; j++) {
				drawingPos += WideSpaceWidth;
			}
			column += (int)((drawingPos + 1) / WideSpaceWidth);
			//
			return drawingPos;
		}
		
		public int GetDrawingXPos(int logicalLine, int logicalColumn)
		{
			
			List<FoldMarker> foldings = Document.FoldingManager.GetTopLevelFoldedFoldings();
			int i;
			FoldMarker f = null;
			//
			for (i = foldings.Count - 1; i >= 0; --i) {
				f = foldings[i];
				if (f.StartLine < logicalLine || f.StartLine == logicalLine && f.StartColumn < logicalColumn) {
					break;
				}
				FoldMarker f2 = foldings[i / 2];
				if (f2.StartLine > logicalLine || f2.StartLine == logicalLine && f2.StartColumn >= logicalColumn) {
					i /= 2;
				}
			}
			
			int column       = 0;
			float drawingPos;
			Graphics g = textArea.CreateGraphics();
			if (f == null || !(f.StartLine < logicalLine || f.StartLine == logicalLine && f.StartColumn < logicalColumn)) {
				drawingPos = CountColumns(ref column, 0, logicalColumn, logicalLine, g);
				return (int)(drawingPos - textArea.VirtualTop.X);
			}
			
			if (f.EndLine > logicalLine || f.EndLine == logicalLine && f.EndColumn > logicalColumn) {
				logicalColumn = f.StartColumn;
				logicalLine = f.StartLine;
				--i;
			}
			for (; i >= 0; --i) {
				f = (FoldMarker)foldings[i];
				if (f.EndLine < logicalLine) { // reached the begin of a new visible line
					break;
				}
			}
			int foldEnd      = 0;
			drawingPos = 0;
			
			drawingPos += CountColumns(ref column, foldEnd, logicalColumn, logicalLine, g);
			g.Dispose();
			return (int)(drawingPos - textArea.VirtualTop.X);
		}
		#endregion
		
		#region DrawHelper functions
		
		void DrawString(Graphics g, string text, Font font, Color color, int x, int y)
		{
			TextRenderer.DrawText(g, text, font, new Point(x, y), color, textFormatFlags);
		}
		
		#endregion
	}
}
