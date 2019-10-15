using System;
using System.Text;

namespace HammingCodeGenerator.Models
{
    class HammingCode
    {
        private int GetParityBitSize(int dataSize)
        {
            int left = 1;
            int right = dataSize;
            int paritySize = 0;
            while (left < right)
            {
                int mid = (left + right) / 2;
                if (Math.Pow(2, mid) >= dataSize + mid + 1)
                {
                    paritySize = mid;
                    right = mid - 1;
                }
                else
                {
                    left = mid + 1;
                }
            }
            return paritySize;
        }

        /// <summary>
        /// Even Parity Hamming Code Generator
        /// </summary>
        public string GenerateHammingCode(ref string data)
        {
            uint hamming_code = 0;
            int paritySize = GetParityBitSize(data.Length);
            int size = paritySize + data.Length;

            int pos = size;
            foreach (char bit in data)
            {
                while ((pos & (pos - 1)) == 0)
                {
                    pos--;
                }
                if (bit == '1')
                {
                    hamming_code |= (1u << (pos - 1));
                }
                pos--;
            }

            int[] parityArr = new int[paritySize];
            for (int i = 1; i <= size; i++)
            {
                for (int j = 0; j < paritySize; j++)
                {
                    if ((i & (1 << j)) != 0)
                    {
                        if ((hamming_code & (1u << (i - 1))) != 0)
                        {
                            parityArr[j]++;
                        }
                    }
                }
            }

            for (int j = 0; j < paritySize; j++)
            {
                if (parityArr[j] % 2 != 0)
                {
                    hamming_code |= 1u << ((int)Math.Pow(2, j) - 1);
                }
            }

            string hamming_str = Convert.ToString(hamming_code, 2).PadLeft(size, '0');
            char[] temp_str = hamming_str.ToCharArray();
            Array.Reverse(temp_str);
            hamming_str = new string(temp_str);
            return hamming_str;
        }

        public int CheckHammingCode(ref string hammingCode)
        {
            int codeSize = hammingCode.Length;
            int correctPos = 0;
            int paritySize = (int)Math.Log(codeSize + 1, 2);
            for (int i = 0; i < paritySize; i++)
            {
                int cnt = 0;
                for (int j = 1; j <= codeSize; j++)
                {
                    if ((j & (1 << i)) != 0)
                    {
                        if (hammingCode[j - 1] == '1')
                        {
                            cnt++;
                        }
                    }
                }
                if (cnt % 2 != 0)
                {
                    correctPos |= (1 << i);
                }
            }
            return correctPos;
        }

        public string CorrectHammingCode(ref string hammingCode, int correctPos)
        {
            StringBuilder sb = new StringBuilder(hammingCode);
            sb[correctPos - 1] = sb[correctPos - 1] == '0' ? '1' : '0';
            return sb.ToString();
        }
    }
}
