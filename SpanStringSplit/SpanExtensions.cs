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

        public static string GetNthToken(this ReadOnlySpan<char> inputSpan, char splitChar, int nthToken)
        {
            if (inputSpan == null)
            {
                return null;
            }
            int[] splitIndexes = inputSpan.SplitIndexes(splitChar, nthToken); 
            if (splitIndexes.Length == 0)
            {
                return inputSpan.ToString();
            }
            if (nthToken == 0 && splitIndexes.Length > 0)
            {
                return inputSpan.Slice(0, splitIndexes[0]).ToString(); 
            }
            if (nthToken > splitIndexes.Length)
            {
                return null; 
            }
            if (nthToken == splitIndexes.Length)
            {
                var split = inputSpan.Slice(splitIndexes[nthToken-1]+1).ToString();
                return split; 
            }
            if (nthToken <= splitIndexes.Length + 1)
            {
                var split = inputSpan.Slice(splitIndexes[nthToken-1]+1, splitIndexes[nthToken] - splitIndexes[nthToken-1]-1).ToString();
                return split; 
            }
            return null; 

        }

        public static int[] SplitIndexes(this ReadOnlySpan<char> inputSpan, char splitChar,
            int? highestSplitIndex = null)
        {
            if (inputSpan == null)
            {
                return Array.Empty<int>();
            }
            if (inputSpan.Length == 0)
            {
                return Array.Empty<int>();
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
                return Array.Empty<int>();
            }
         
            var splitIndexes = new List<int>();
            var tokens = new List<string>();
            int charIndex = 0;
            foreach (var ch in inputSpan)
            {
                if (ch == splitChar)
                {
                    if (highestSplitIndex.HasValue && highestSplitIndex + 1 < splitIndexes.Count)
                    {
                        break; 
                    }
                    splitIndexes.Add(charIndex);
                }
                charIndex++; 
            }
            return splitIndexes.ToArray(); 
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
