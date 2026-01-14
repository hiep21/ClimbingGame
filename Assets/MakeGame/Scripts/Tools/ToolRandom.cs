using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NTHiep.Tool
{
    public class RandomUtil
    {
        [ThreadStatic]
        private static ulong threadSeed;

        private static ulong InitSeed()
        {
            ulong timeSeed = (ulong)DateTime.UtcNow.Ticks;
            ulong threadId = (ulong)Thread.CurrentThread.ManagedThreadId;
            return (timeSeed ^ (threadId << 32)) | 1UL; // XOR thời gian với Thread ID, tránh seed = 0
        }

        public static void SetSeed(ulong s)
        {
            threadSeed = (s != 0 ? s : 1UL);
        }

        private static ulong NextULong()
        {
            if (threadSeed == 0)
                threadSeed = InitSeed();

            threadSeed ^= threadSeed >> 12;
            threadSeed ^= threadSeed << 25;
            threadSeed ^= threadSeed >> 27;
            return threadSeed * 0x2545F4914F6CDD1DUL;
        }
        static float NextFloat01()
        {
            return (float)((NextULong() >> 11) * (1.0 / (1UL << 53))); // Tối đa độ mịn của double
        }

        /// <summary>
        /// Trả về chuỗi ngẫu nhiên
        /// </summary>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*-_=+/?";
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[Range(0, chars.Length)];
            }
            return new string(stringChars);
        }

        /// <summary>
        /// Trả về số nguyên ngẫu nhiên trong khoảng <b>[0, max)</b> (bao gồm 0, loại trừ max).
        /// </summary>
        /// <param name="max">Giá trị lớn nhất (loại trừ)</param>
        /// <returns>Số nguyên trong khoảng <b>[0, max)</b></returns>
        public static int Range(int max)
        {
            if (0 >= max)
                throw new ArgumentException("min phải nhỏ hơn max");

            return (int)(NextULong() % (ulong)(max - 0)) + 0;
        }


        /// <summary>
        /// Trả về số nguyên ngẫu nhiên trong khoảng <b>[min, max)</b> (bao gồm min, loại trừ max).
        /// </summary>
        /// <param name="min">Giá trị nhỏ nhất (bao gồm)</param>
        /// <param name="max">Giá trị lớn nhất (loại trừ)</param>
        /// <returns>Số nguyên trong khoảng <b>[min, max)</b></returns>
        public static int Range(int min, int max)
        {
            if (min >= max)
                throw new ArgumentException("min phải nhỏ hơn max");

            return (int)(NextULong() % (ulong)(max - min)) + min;
        }



        /// <summary>
        /// Trả về một số thực ngẫu nhiên trong khoảng <b>[min, max)</b> 
        /// </summary>
        /// <param name="min">Giá trị nhỏ nhất (bao gồm)</param>
        /// <param name="max">Giá trị lớn nhất (loại trừ)</param>
        /// <returns>Số thực ngẫu nhiên trong khoảng <b>[min, max)</b></returns>
        public static float Range(float min, float max)
        {
            return min + (max - min) * NextFloat01();
        }

        /// <summary>
        /// Trả về một số nguyên ngẫu nhiên trong khoảng <b>[min, max]</b> (cả hai đầu bao gồm).
        /// </summary>
        /// <param name="min">Giá trị nhỏ nhất (bao gồm)</param>
        /// <param name="max">Giá trị lớn nhất (bao gồm)</param>
        /// <returns>Số nguyên ngẫu nhiên trong khoảng <b>[min, max]</b></returns>
        public static int RangeInclusive(int min, int max)
        {
            return Range(min, max + 1);
        }

        public static int GetRandomIndexByRate(List<int> rates, int totalRate)
        {
            List<List<int>> listRates = new List<List<int>>();
            List<int> listRated = new List<int>();
            foreach (var item in rates)
            {
                List<int> listRate = new List<int>();
                listRate = ConvertNumberToRate(item, totalRate, listRated);

                listRated.AddRange(listRate);
                listRates.Add(listRate);
            }

            int randomIndex = Range(0, totalRate);
            foreach (var item in listRates)
            {
                if (item.Any(f => f == randomIndex))
                {
                    return listRates.FindIndex(f => f == item);
                }
            }
            return -1;

        }


        static List<int> ConvertNumberToRate(int numberRate, int totalRate, List<int> listRated)
        {
            List<int> resultConvert = new List<int>();
            for (int i = 0; i < numberRate; i++)
            {
                int randomNumber = Range(0, totalRate);
                if (!resultConvert.Any(x => x == randomNumber) && !listRated.Any(x => x == randomNumber))
                {
                    resultConvert.Add(randomNumber);
                }
                else
                {
                    i--;
                }
            }
            return resultConvert;
        }

        // FLOAT: [0, 1)
        /// <summary>
        /// Trả về một số thực ngẫu nhiên trong khoảng <b>[0, 1)</b> 
        /// </summary>
        /// <returns>Số thực ngẫu nhiên trong khoảng <b>[0, 1)</b></returns>
        public static float Value()
        {
            return Range(0f, 1f);
        }

        // BOOL: 50/50
        /// <summary>
        /// Trả về một bool ngẫu nhiên với xác suất 50%
        /// </summary>
        /// <returns>bool ngẫu nhiên với xác suất 50%</returns>
        public static bool RandomBool()
        {
            return (NextULong() % 2) == 0;
        }

        // VECTOR2: random từ min đến max
        /// <summary>
        /// Trả về một Vector2 ngẫu nhiên trong khoảng <b>[min, max)</b> 
        /// </summary>
        /// <param name="min">Giá trị nhỏ nhất (bao gồm)</param>
        /// <param name="max">Giá trị lớn nhất (loại trừ)</param>
        /// <returns>Vector2 ngẫu nhiên trong khoảng <b>[min, max)</b></returns>
        public static Vector2f RangeVector2(Vector2f min, Vector2f max)
        {
            return new Vector2f(
                Range(min.x, max.x),
                Range(min.y, max.y)
            );
        }

        // VECTOR3: random từ min đến max
        /// <summary>
        /// Trả về một Vector3 ngẫu nhiên trong khoảng <b>[min, max)</b> 
        /// </summary>
        /// <param name="min">Giá trị nhỏ nhất (bao gồm)</param>
        /// <param name="max">Giá trị lớn nhất (loại trừ)</param>
        /// <returns>Vector3 ngẫu nhiên trong khoảng <b>[min, max)</b></returns>
        public static Vector3f RangeVector3(Vector3f min, Vector3f max)
        {
            return new Vector3f(
                Range(min.x, max.x),
                Range(min.y, max.y),
                Range(min.z, max.z)
            );
        }


        // SHUFFLE: List<T>
        /// <summary>
        /// Xáo trộn một List<T>
        /// </summary>
        /// <param name="list">List<T> cần shuffle</param>
        /// <returns>List<T> đã được xáo trộn</returns>
        public static void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Range(0, i + 1);
                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        // SHUFFLE: Array
        /// <summary>
        /// Xáo trộn một mảng
        /// </summary>
        /// <param name="array">Mảng cần shuffle</param>
        public static void Shuffle<T>(T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = Range(0, i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        // PICK: random 1 phần tử từ List<T>
        /// <summary>
        /// Trả về một phần tử ngẫu nhiên từ List<T>
        /// </summary>
        /// <param name="list">List<T> cần random</param>
        /// <returns>Phần tử ngẫu nhiên từ List<T></returns>
        public static T Pick<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List rỗng hoặc null.");
            int index = Range(0, list.Count);
            return list[index];
        }


        /// <summary>
        /// Random 1 phần tử từ List<T> rồi xóa nó khỏi list
        /// </summary>
        /// <param name="list">Danh sách cần bốc thăm và xóa</param>
        /// <returns>Phần tử đã chọn và bị loại khỏi danh sách</returns>
        public static T PickAndRemove<T>(List<T> list)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List rỗng hoặc null.");

            int index = Range(0, list.Count);
            T item = list[index];
            list.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Random 1 list phần tử từ List<T> rồi xóa nó khỏi list
        /// </summary>
        /// <param name="list">Danh sách cần bốc thăm và xóa</param>
        /// <returns>List phần tử đã chọn và bị loại khỏi danh sách</returns>
        public static List<T> PickListAndRemove<T>(List<T> list, int count)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Danh sách null hoặc rỗng.");

            if (count <= 0 || count > list.Count)
                throw new ArgumentOutOfRangeException("Số lượng phần tử cần chọn không hợp lệ.");

            List<T> result = new List<T>();

            for (int i = 0; i < count; i++)
            {
                int index = Range(0, list.Count);
                result.Add(list[index]);
                list.RemoveAt(index);
            }

            return result;
        }

        /// <summary>
        /// Trả về một danh sách gồm n phần tử ngẫu nhiên không trùng từ list
        /// </summary>
        /// <param name="list">Danh sách đầu vào</param>
        /// <param name="count">Số phần tử cần chọn</param>
        /// <returns>Danh sách chứa các phần tử được chọn</returns>
        public static List<T> PickWithNumber<T>(List<T> list, int count)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Danh sách null hoặc rỗng.");

            if (count <= 0 || count > list.Count)
                throw new ArgumentOutOfRangeException("Số lượng phần tử cần chọn không hợp lệ.");

            List<T> temp = new List<T>(list);
            Shuffle(temp);

            return temp.GetRange(0, count);
        }



        /// <summary>
        /// Trả về phần tử ngẫu nhiên từ List<T>, loại trừ các phần tử được chỉ định
        /// </summary>
        /// <param name="list">Danh sách cần chọn</param>
        /// <param name="exclude">Các phần tử cần loại</param>
        /// <returns>Phần tử ngẫu nhiên không nằm trong exclude</returns>
        public static T PickExclude<T>(List<T> list, List<T> exclude)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("Danh sách null hoặc rỗng.");

            List<T> candidates = new List<T>();
            foreach (T item in list)
            {
                if (!exclude.Contains(item))
                    candidates.Add(item);
            }

            if (candidates.Count == 0)
                return default(T);

            int index = Range(0, candidates.Count);
            return candidates[index];
        }

        /// <summary>
        /// Trả về các list<T> đã được chia đều
        /// </summary>
        /// <param name="list">Danh sách cần chọn</param>
        /// <param name="numberOfLists">Số list cần chia</param>
        /// <returns>các list chia đều có các phần tử ngẫu nhiên</returns>
        public static List<List<T>> SplitList<T>(List<T> list, int numberOfLists = 2)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List null hoặc rỗng.");

            if (numberOfLists <= 0)
                throw new ArgumentOutOfRangeException("Số lượng list phải lớn hơn 0.");

            if (numberOfLists > list.Count)
                throw new ArgumentOutOfRangeException("Số lượng list không được lớn hơn số phần tử trong list.");

            // Shuffle để random các phần tử trước khi chia (nếu cần random)
            // Nếu không muốn random thì bỏ dòng này đi
            List<T> temp = new List<T>(list);
            Shuffle(temp);

            List<List<T>> result = new List<List<T>>();

            int baseSize = list.Count / numberOfLists;
            int extra = list.Count % numberOfLists; // mấy phần dư chia đều cho các list đầu tiên

            int index = 0;

            for (int i = 0; i < numberOfLists; i++)
            {
                int size = baseSize + (i < extra ? 1 : 0);
                result.Add(temp.GetRange(index, size));
                index += size;
            }

            return result;
        }

        /// <summary>
        /// Trả về 2 list muốn lấy random phần tử và các phần tử còn lại
        /// </summary>
        /// <param name="list">Danh sách cần chọn</param>
        /// <param name="pickCount">Số phần tử cần chọn trong list</param>
        /// <returns>1 list cần chọn và 1 list còn lại</returns>
        public static (List<T> picked, List<T> rest) SplitTwoListRandom<T>(List<T> list, int pickCount)
        {
            if (list == null || list.Count == 0)
                throw new ArgumentException("List null hoặc rỗng.");

            if (pickCount < 0 || pickCount > list.Count)
                throw new ArgumentOutOfRangeException(nameof(pickCount), "Số phần tử cần chọn không hợp lệ.");

            // copy để không phá list gốc
            var temp = new List<T>(list);

            // shuffle random
            FisherYatesShuffle(temp);

            // lấy picked và phần còn lại
            var picked = temp.GetRange(0, pickCount);
            var rest = temp.GetRange(pickCount, temp.Count - pickCount);
            return (picked, rest);


            void FisherYatesShuffle(List<T> list)
            {
                for (int i = list.Count - 1; i > 0; i--)
                {
                    int j = Range(i + 1);
                    T tmp = list[i];
                    list[i] = list[j];
                    list[j] = tmp;
                }
            }
        }



        /// <summary>
        /// Trả về một giá trị enum ngẫu nhiên
        /// </summary>
        /// <returns>Giá trị enum ngẫu nhiên</returns>
        public static T RandomEnum<T>() where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int index = Range(0, values.Length);
            return (T)values.GetValue(index);
        }

        /// <summary>
        /// Trả về một giá trị enum ngẫu nhiên từ khoảng chỉ định (start index -> end index)
        /// </summary>
        /// <typeparam name="T">Kiểu enum</typeparam>
        /// <param name="startIndex">Chỉ số bắt đầu (tính từ 0)</param>
        /// <param name="endIndex">Chỉ số kết thúc (bao gồm)</param>
        public static T RandomEnumRange<T>(int startIndex, int endIndex) where T : Enum
        {
            Array values = Enum.GetValues(typeof(T));
            int length = values.Length;

            if (startIndex < 0 || endIndex >= length || startIndex > endIndex)
            {
                Debug.LogWarning("Chỉ số start hoặc end không hợp lệ.");
                return default;
            }

            int index = Range(startIndex, endIndex + 1); // end + 1 vì Random.Range loại trừ
            return (T)values.GetValue(index);
        }


        /// <summary>
        /// Trả về một giá trị enum ngẫu nhiên, loại trừ các giá trị được chỉ định
        /// </summary>
        /// <param name="exclude">Các giá trị enum cần loại trừ</param>
        /// <returns>Giá trị enum ngẫu nhiên, loại trừ các giá trị được chỉ định</returns>
        public static T RandomEnumExclude<T>(params T[] exclude) where T : Enum
        {
            Array allValues = Enum.GetValues(typeof(T));
            List<T> validValues = new List<T>();

            foreach (T value in allValues)
            {
                if (Array.IndexOf(exclude, value) == -1)
                    validValues.Add(value);
            }

            if (validValues.Count == 0)
                throw new InvalidOperationException("Không còn giá trị enum sau khi loại trừ.");

            int index = Range(0, validValues.Count);
            return validValues[index];
        }


        /// <summary>
        /// Random một giá trị enum trong khoảng chỉ định, loại trừ các giá trị cho trước.
        /// </summary>
        /// <typeparam name="T">Kiểu enum</typeparam>
        /// <param name="startIndex">Chỉ số bắt đầu (tính từ 0)</param>
        /// <param name="endIndex">Chỉ số kết thúc (bao gồm)</param>
        /// <param name="excludes">Danh sách các giá trị enum cần loại trừ</param>
        /// <returns>Một giá trị enum ngẫu nhiên thỏa điều kiện, hoặc default nếu không có giá trị hợp lệ</returns>
        public static T RandomEnumRangeExclude<T>(int startIndex, int endIndex, params T[] excludes) where T : Enum
        {
            Array allValues = Enum.GetValues(typeof(T));
            int totalLength = allValues.Length;

            if (startIndex < 0 || endIndex >= totalLength || startIndex > endIndex)
            {
                Debug.LogWarning("Chỉ số start hoặc end không hợp lệ.");
                return default;
            }

            List<T> filtered = new List<T>();

            for (int i = startIndex; i <= endIndex; i++)
            {
                T val = (T)allValues.GetValue(i);
                if (Array.IndexOf(excludes, val) == -1)
                {
                    filtered.Add(val);
                }
            }

            if (filtered.Count == 0)
            {
                Debug.LogWarning("Không còn giá trị enum nào sau khi loại trừ.");
                return default;
            }

            int randomIndex = Range(0, filtered.Count);
            return filtered[randomIndex];
        }

        /// <summary>
        /// Random một giá trị enum trong danh sách được chỉ định
        /// </summary>
        /// <typeparam name="T">Kiểu enum</typeparam>
        /// <param name="options">Các giá trị enum được chọn để random</param>
        /// <returns>Một giá trị enum ngẫu nhiên từ danh sách options</returns>
        public static T RandomEnumFrom<T>(params T[] options) where T : Enum
        {
            if (options == null || options.Length == 0)
            {
                Debug.LogWarning("Danh sách enum đầu vào rỗng.");
                return default;
            }

            int index = Range(0, options.Length);
            return options[index];
        }

    }
    public struct Vector2f
    {
        public float x;
        public float y;

        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString() => $"({x}, {y})";
    }
    public struct Vector3f
    {
        public float x;
        public float y;
        public float z;

        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString() => $"({x}, {y}, {z})";
    }

    /// <summary>
    /// Công cụ hỗ trợ xáo trộn mảng đa chiều và danh sách lồng nhau
    /// </summary>
    public static class ToolRandomND
    {
        /// <summary>
        /// Shuffle mảng n chiều (Array đa chiều: [,], [,,], ...)
        /// </summary>
        public static void Shuffle(Array arr)
        {
            if (arr == null || arr.Length == 0)
                throw new ArgumentException("Array is null or empty.");

            // Flatten array
            List<object> flat = new List<object>();
            foreach (var item in arr)
                flat.Add(item);

            // Shuffle
            RandomUtil.Shuffle(flat);

            // Gán lại vào array đa chiều
            int index = 0;
            foreach (var idx in arr.Indices())
            {
                arr.SetValue(flat[index++], idx);
            }
        }

        /// <summary>
        /// Shuffle list lồng nhau n cấp, giữ nguyên cấu trúc
        /// </summary>
        public static void ShuffleRecursive<T>(IList list)
        {
            // Flatten list n cấp
            List<T> flat = new List<T>();
            Flatten<T>(list, flat);

            // Shuffle
            RandomUtil.Shuffle(flat);

            // Gán lại
            int index = 0;
            Refill<T>(list, flat, ref index);
        }

        /// <summary>
        /// Shuffle list lồng nhau n cấp, giữ nguyên cấu trúc, trả về danh sách mới
        /// </summary>
        private static void Flatten<T>(IList source, List<T> result)
        {
            foreach (var item in source)
            {
                if (item is IList subList && !(item is string))
                    Flatten<T>(subList, result);
                else
                    result.Add((T)item);
            }
        }

        /// <summary>
        /// Gán lại giá trị đã shuffle vào danh sách lồng nhau n cấp
        /// </summary>
        private static void Refill<T>(IList target, List<T> flat, ref int index)
        {
            for (int i = 0; i < target.Count; i++)
            {
                if (target[i] is IList subList && !(target[i] is string))
                {
                    Refill<T>((IList)target[i], flat, ref index);
                }
                else
                {
                    target[i] = flat[index++];
                }
            }
        }

        /// <summary>
        /// Extension: tạo IEnumerable tất cả index cho mảng n chiều
        /// </summary>
        private static IEnumerable<int[]> Indices(this Array array)
        {
            int[] lengths = new int[array.Rank];
            for (int i = 0; i < lengths.Length; i++)
                lengths[i] = array.GetLength(i);

            int[] current = new int[lengths.Length];
            while (true)
            {
                yield return (int[])current.Clone();

                int d = lengths.Length - 1;
                while (d >= 0)
                {
                    current[d]++;
                    if (current[d] < lengths[d]) break;
                    current[d] = 0;
                    d--;
                }
                if (d < 0) break;
            }
        }
    }

}