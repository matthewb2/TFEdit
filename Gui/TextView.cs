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
		int          fontHeight;
		Highlight    highlight;
		
		public void Dispose()
		{
			measureCache.Clear();
		}
		
		public Highlight Highlight {
			get {
				return highlight;
			}
			set {
				highlight = value;
			}
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
		
		public int VisibleColumnCount {
			get {
				return (int)(DrawingPosition.Width / WideSpaceWidth) - 1;
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
			Console.WriteLine("font height:" + height1);
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
			// 
			this.spaceWidth = Math.Max(GetWidth(' ', lastFont), 1);
			//
			this.wideSpaceWidth = Math.Max(spaceWidth, GetWidth('x', lastFont));
		}
		
		#region Paint functions
		public override void Paint(Graphics g, Rectangle rect)
		{
			if (rect.Width <= 0 || rect.Height <= 0) {
				return;
			}
			//
			Console.WriteLine(DrawingPosition.X + " " + DrawingPosition.Y + " " + DrawingPosition.Height);
			Console.WriteLine(DrawingPosition.Left + " " + DrawingPosition.Right + " " + DrawingPosition.Bottom);

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
			
			Brush bgColorBrush    = GetBgColorBrush(lineNumber);
			Brush backgroundBrush = textArea.Enabled ? bgColorBrush : SystemBrushes.InactiveBorder;
			
			if (lineNumber >= textArea.Document.TotalNumberOfLines) {
					g.FillRectangle(backgroundBrush, lineRectangle);
				return;
			}
			
			int physicalXPos = lineRectangle.X;
			
			physicalXPos = PaintLinePart(g, lineNumber, 0, textArea.Document.GetLineSegment(lineNumber).Length, lineRectangle, physicalXPos);
			
			if (lineNumber < textArea.Document.TotalNumberOfLines) {
				// 
				ColumnRange    selectionRange = textArea.SelectionManager.GetSelectionAtLine(lineNumber);
				LineSegment    currentLine    = textArea.Document.GetLineSegment(lineNumber);
								
				bool  selectionBeyondEOL = selectionRange.EndColumn > currentLine.Length || ColumnRange.WholeColumn.Equals(selectionRange);
				Brush fillBrush = selectionBeyondEOL && TextEditorProperties.AllowCaretBeyondEOL ? bgColorBrush : backgroundBrush;
				g.FillRectangle(fillBrush, new RectangleF(physicalXPos, lineRectangle.Y, lineRectangle.Width - physicalXPos + lineRectangle.X, lineRectangle.Height));
			}
		}
		
		
		Brush GetBgColorBrush(int lineNumber)
		{
		
			HighlightColor background = textArea.Document.HighlightingStrategy.GetColorFor("Default");
			Color bgColor = background.BackgroundColor;
			return BrushRegistry.GetBrush(bgColor);
		
		}
		
		
		/// <summary>
		/// Get the marker brush (for solid block markers) at a given position.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="length">The length.</param>
		/// <param name="markers">All markers that have been found.</param>
		/// <returns>The Brush or null when no marker was found.</returns>
		Brush GetMarkerBrushAt(int offset, int length, ref Color foreColor, out IList<TextMarker> markers)
		{
			markers = Document.MarkerStrategy.GetMarkers(offset, length);
			foreach (TextMarker marker in markers) {
				if (marker.TextMarkerType == TextMarkerType.SolidBlock) {
					if (marker.OverrideForeColor) {
						foreColor = marker.ForeColor;
					}
					return BrushRegistry.GetBrush(marker.Color);
				}
			}
			return null;
		}
		
		int PaintLinePart(Graphics g, int lineNumber, int startColumn, int endColumn, Rectangle lineRectangle, int physicalXPos)
		{

			LineSegment currentLine    = textArea.Document.GetLineSegment(lineNumber);
			
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
				
				IList<TextMarker> markers;
				Color wordForeColor = currentWord.Color;
				
				Brush wordBackBrush = GetMarkerBrushAt(currentLine.Offset + currentWordOffset, currentWord.Length, ref wordForeColor, out markers);
				
				int wordWidth = DrawDocumentWord(g,
												 currentWord.Word,
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
			
			if (word.Length > MaximumWordLength) {
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
			int wordWidth = MeasureStringWidth(g, word, font);
			g.FillRectangle(backBrush, new RectangleF(position.X, position.Y, wordWidth + 1, FontHeight));
			//
			DrawString(g,
			           word,
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
			public override bool Equals(object obj) {
				WordFontPair myWordFontPair = (WordFontPair)obj;
				if (!word.Equals(myWordFontPair.word)) return false;
				return font.Equals(myWordFontPair.font);
			}
			
			public override int GetHashCode() {
				return word.GetHashCode() ^ font.GetHashCode();
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
		
		public int GetVisualColumnFast(LineSegment line, int logicalColumn)
		{
			int lineOffset = line.Offset;
			int tabIndent = Document.TextEditorProperties.TabIndent;
			int guessedColumn = 0;
			for (int i = 0; i < logicalColumn; ++i) {
				char ch;
				if (i >= line.Length) {
					ch = ' ';
				} else {
					ch = Document.GetCharAt(lineOffset + i);
				}
				++guessedColumn;
			
			}
			return guessedColumn;
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
			
			int start = 0; // column
			int posX = 0; // visual position
			
			int result;
			using (Graphics g = textArea.CreateGraphics()) {
				// 
				while (true) {
					//
					LineSegment line = Document.GetLineSegment(lineNumber);
					FoldMarker nextFolding = FindNextFoldedFoldingOnLineAfterColumn(lineNumber, start-1);
					int end = nextFolding != null ? nextFolding.StartColumn : int.MaxValue;
					result = GetLogicalColumnInternal(g, line, start, end, ref posX, visualPosX);
					
					if (result < end)
						break;
					
					lineNumber = nextFolding.EndLine;
					start = nextFolding.EndColumn;
					int newPosX = posX + 1 + MeasureStringWidth(g, nextFolding.FoldText, TextEditorProperties.FontContainer.RegularFont);
					//
					if (newPosX >= visualPosX) {
						inFoldMarker = nextFolding;
						if (IsNearerToAThanB(visualPosX, posX, newPosX))
							return new TextLocation(nextFolding.StartColumn, nextFolding.StartLine);
						else
							return new TextLocation(nextFolding.EndColumn, nextFolding.EndLine);
					}
					posX = newPosX;
				}
			}
			return new TextLocation(result, lineNumber);
		}
		
		int GetLogicalColumnInternal(Graphics g, LineSegment line, int start, int end, ref int drawingPos, int targetVisualPosX)
		{
			if (start == end)
				return end;
		
			FontContainer fontContainer = TextEditorProperties.FontContainer;
			List<TextWord> words = line.Words;
			if (words == null) return 0;
			int wordOffset = 0;
			for (int i = 0; i < words.Count; i++) {
				TextWord word = words[i];
				if (wordOffset >= end) {
					return wordOffset;
				}
				if (wordOffset + word.Length >= start) {
					int newDrawingPos;
					int wordStart = Math.Max(wordOffset, start);
					int wordLength = Math.Min(wordOffset + word.Length, end) - wordStart;
					string text = Document.GetText(line.Offset + wordStart, wordLength);
					Font font = word.GetFont(fontContainer) ?? fontContainer.RegularFont;
					newDrawingPos = drawingPos + MeasureStringWidth(g, text, font);
					if (newDrawingPos >= targetVisualPosX)
					{
						for (int j = 0; j < text.Length; j++)
						{
							newDrawingPos = drawingPos + MeasureStringWidth(g, text[j].ToString(), font);
							if (newDrawingPos >= targetVisualPosX)
							{
								if (IsNearerToAThanB(targetVisualPosX, drawingPos, newDrawingPos))
									return wordStart + j;
								else
									return wordStart + j + 1;
							}
							drawingPos = newDrawingPos;
						}
						return wordStart + text.Length;
					}
					drawingPos = newDrawingPos;
				}
				wordOffset += word.Length;
			}
			return wordOffset;
		}
		
		static bool IsNearerToAThanB(int num, int a, int b)
		{
			return Math.Abs(a - num) < Math.Abs(b - num);
		}
		FoldMarker FindNextFoldedFoldingOnLineAfterColumn(int lineNumber, int column)
		{
			List<FoldMarker> list = Document.FoldingManager.GetFoldedFoldingsWithStartAfterColumn(lineNumber, column);
			if (list.Count != 0)
				return list[0];
			else
				return null;
		}
		
		
		float CountColumns(ref int column, int start, int end, int logicalLine, Graphics g)
		{
			if (start > end) throw new ArgumentException("start > end");
			if (start == end) return 0;
			//
			float drawingPos = 0;
			//
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
			//
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
