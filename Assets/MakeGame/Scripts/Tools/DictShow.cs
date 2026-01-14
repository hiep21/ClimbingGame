using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace NTHiep.Tool
{
    [System.Serializable]
    public class DictShow<Key, Value> : ISerializationCallbackReceiver
    {
        public Dictionary<Key, Value> Dictionary = new Dictionary<Key, Value>();

        // Chỉ dùng để show lên Inspector
        [SerializeField] List<Key> Keys = new List<Key>();
        [SerializeField] List<Value> Values = new List<Value>();

        public int Count => Dictionary.Count;

        // Inspector → Runtime
        public void OnAfterDeserialize()
        {
            Dictionary.Clear();

            int count = Mathf.Min(Keys.Count, Values.Count);
            for (int i = 0; i < count; i++)
            {
                // Không check null ở generic
                Dictionary[Keys[i]] = Values[i];
            }
        }

        // Runtime → Inspector
        public void OnBeforeSerialize()
        {
            // Keys.Clear();
            // Values.Clear();

            // foreach (var kv in Dictionary)
            // {
            //     Keys.Add(kv.Key);
            //     Values.Add(kv.Value);
            // }
        }
#if UNITY_EDITOR
        public void EditorRebuild()
        {
            Dictionary.Clear();
            int count = Mathf.Min(Keys.Count, Values.Count);
            for (int i = 0; i < count; i++)
                Dictionary[Keys[i]] = Values[i];
        }
#endif

        /// <summary>
        /// Xóa hết data dictionary
        /// </summary>
        public void Clear()
        {
            Dictionary.Clear();

#if UNITY_EDITOR
            Keys.Clear();
            Values.Clear();
#endif
        }

        /// <summary>
        /// Thêm data vào dictionary
        /// </summary>
        /// <param name="key">Khóa để lấy value</param>
        /// <param name="value"></param>
        public void Add(Key key, Value value)
        {
            Dictionary[key] = value;

#if UNITY_EDITOR
            int index = Keys.IndexOf(key);

            if (index != -1)
            {
                // Key đã tồn tại → update value
                Values[index] = value;
            }
            else
            {
                // Key mới → thêm mới vào list
                Keys.Add(key);
                Values.Add(value);
            }
#endif
        }
        /// <summary>
        /// Lấy value theo key
        /// </summary>
        /// <param name="key">Khóa để lấy value</param>
        /// <returns>Value</returns>
        public Value Get(Key key)
        {
            if (Dictionary.TryGetValue(key, out Value value))
                return value;

            return default;
        }

        /// <summary>
        /// Lấy key theo value
        /// </summary>
        /// <param name="value">Value để lấy key</param>
        /// <returns>Key</returns>
        public Key GetKey(Value value)
        {
            foreach (var kv in Dictionary)
            {
                if (EqualityComparer<Value>.Default.Equals(kv.Value, value))
                    return kv.Key;
            }

            return default;
        }




        /// <summary>
        /// Kiểm tra key có tồn tại trong Dict không
        /// </summary>
        /// <param name="key">Khóa để kiểm tra</param>
        /// <returns><True> hoặc <False></returns>
        public bool Contains(Key key)
        {
            return Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// Xóa key và value theo key
        /// </summary>
        /// <param name="key">Khóa để xóa</param>
        public void Remove(Key key)
        {
            if (key == null)
                return;

            if (Dictionary.Remove(key))
            {
#if UNITY_EDITOR
                int index = Keys.IndexOf(key);
                if (index != -1)
                {
                    Keys.RemoveAt(index);
                    Values.RemoveAt(index);
                }
#endif
            }
        }
        /// <summary>
        /// Chuyển đổi Dict sang List<Value>
        /// </summary>
        /// <returns>List<Value></returns>
        public List<Value> ToList()
        {
            // Trả về theo thứ tự Keys để dễ debug
            List<Value> list = new List<Value>();

#if UNITY_EDITOR
            for (int i = 0; i < Keys.Count; i++)
            {
                if (Dictionary.TryGetValue(Keys[i], out Value value))
                    list.Add(value);
            }
#else
            // Runtime không cần quan tâm thứ tự
            foreach (var kvp in Dictionary)
                list.Add(kvp.Value);
#endif

            return list;
        }

    }
}
