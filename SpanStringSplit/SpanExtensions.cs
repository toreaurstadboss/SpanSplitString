using System;
using System.Collections.Generic;

namespace SpanStringSplit
{
    public static class SpanExtensions
    {

        public static string[] SplitViaSpan(this string input, char splitChar, StringSplitOptions splitOptions)
        {
            if (string.IsNullOrWhiteSpace(input) || input.IndexOf(splitChar) < 0)
            {
                return new string[] { input };
            }
            var tokens = SplitSpan(input.AsSpan(), splitChar, splitOptions);
            return tokens; 
        }

        public static string[] SplitSpan(this ReadOnlySpan<char> inputSpan, char splitChar, StringSplitOptions splitOptions)
        {
            if (inputSpan == null)
            {
                return new string[] { null };
            }
            if (inputSpan.Length == 0)
            {
                return splitOptions == StringSplitOptions.None ? new string[] { string.Empty } : new string[0]; 
            }
            bool isSplitCharFound = false; 
            foreach (char letter in inputSpan)
            {
                if (letter == splitChar)
                {
                    isSplitCharFound = true;
                    break;
                }
            }
            if (!isSplitCharFound)
            {
                return new string[] { inputSpan.ToString() }; 
            }

            bool IsTokenToBeAdded(string token) => !string.IsNullOrWhiteSpace(token) || splitOptions == StringSplitOptions.None;

            var splitIndexes = new List<int>();
            var tokens = new List<string>();
            int charIndx = 0;
            foreach (var ch in inputSpan)
            {
                if (ch == splitChar)
                {
                    splitIndexes.Add(charIndx);
                }
                charIndx++;
            }
            int currentSplitIndex = 0;
            foreach (var indx in splitIndexes)
            {
                if (currentSplitIndex == 0)
                {
                    string firstToken = inputSpan.Slice(0, splitIndexes[0]).ToString();
                    if (IsTokenToBeAdded(firstToken))
                    {
                        tokens.Add(firstToken);
                    }
                }
                else if (currentSplitIndex <= splitIndexes.Count)
                {
                    string intermediateToken = inputSpan.Slice(splitIndexes[currentSplitIndex - 1] + 1, splitIndexes[currentSplitIndex] - splitIndexes[currentSplitIndex - 1] - 1).ToString();
                    if (IsTokenToBeAdded(intermediateToken))
                    {
                        tokens.Add(intermediateToken);
                    }
                }
                currentSplitIndex++;
            }
            string lastToken = inputSpan.Slice(splitIndexes[currentSplitIndex - 1] + 1).ToString();
            if (IsTokenToBeAdded(lastToken))
            {
                tokens.Add(lastToken);
            }
            return tokens.ToArray();
        }

    }
}
