﻿using Mafi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ProgramableNetwork.Python
{
    internal class Expressions
    {
        internal static bool __bool__(object v)
        {
            return v is bool b ? b
                : v is int i ? i != 0
                : v is float f ? f != 0
                : v is Fix32 fix ? fix.RawValue != 0
                : throw new NotImplementedException();
        }

        internal static int __int__(object v)
        {
            return v is int i ? i
                : v is float f ? (int)f
                : v is Fix32 fix ? fix.IntegerPart
                : v is bool b ? (b ? 1 : 0)
                : throw new NotImplementedException();
        }

        internal static float __float__(object v)
        {
            return v is float f ? f
                : v is Fix32 fix ? fix.ToFloat()
                : v is int i ? i
                : v is bool b ? (b ? 1 : 0)
                : throw new NotImplementedException();
        }

        internal static Fix32 __fix__(object v)
        {
            return v is Fix32 fix ? fix
                : v is float f ? f.ToFix32()
                : v is int i ? i.ToFix32()
                : v is bool b ? (b ? 1.ToFix32() : 0.ToFix32())
                : throw new NotImplementedException();
        }

        internal static string __str__(object v)
        {
            if (v is null) return "None";
            if (v is string s) return s;
            if (v is int i) return i.ToString();
            if (v is float f) return f.ToString();
            if (v is Fix32 fix) return fix.ToString();
            System.Reflection.MethodInfo m;
            if ((m = v.GetType().GetMethod("__str__")) != null) return (string)m.Invoke(v, null);
            return v.ToString(); // nouzovka
        }

        internal static object __neg__(object v)
        {
            return v is float f ? -f
                : v is Fix32 fix ? (object)(-fix)
                : v is int i ? -i
                : v is bool b ? (b ? -1 : 0)
                : throw new NotImplementedException();
        }

        internal static object __pos__(object v)
        {
            return v is float f ? f
                : v is Fix32 fix ? (object)(fix)
                : v is int i ? i
                : v is bool b ? (b ? 1 : 0)
                : throw new NotImplementedException();
        }

        internal static object __or__(object v1, object v2)
        {
            if (v1 is null && v2 is null)
                return 0;
            return __int__(v1) | __int__(v2);
        }

        internal static object __xor__(object v1, object v2)
        {
            if (v1 is null && v2 is null)
                return 0;
            return __int__(v1) ^ __int__(v2);
        }

        internal static object __call__(object executable, List<(string name, object value)> arguments)
        {
            if (executable is Constructor constructor)
            {
                return constructor.Invoke(arguments.Select(a =>
                    a.name == null
                        ? (IArgumentValue)new OrderedValue(a.value)
                        : (IArgumentValue)new NamedValue(a.name, a.value))
                    .ToArray());
            }
            if (executable is MemberCall member)
            {
                ParameterInfo[] parameters = member.Type[0].GetParameters();
                object[] values = arguments
                    .Select(a => a.value)
                    .ToArray();
                return member.Type[0].Invoke(member.Target, values);
            }
            if (executable is Function function)
            {
                return function.Invoke(
                    new IArgumentValue[] {
                        new OrderedValue(function.Self)
                    }.Concat(arguments.Select(a =>
                        a.name == null
                            ? (IArgumentValue)new OrderedValue(a.value)
                            : (IArgumentValue)new NamedValue(a.name, a.value))
                    ).ToArray());
            }
            throw new NotImplementedException();
        }

        internal static bool __eq__(object left, object right)
        {
            if (left is null && right is null)
                return true;
            if (left is null || right is null)
                return false;
            if (left.GetType() != right.GetType())
            {
                if (__eq__base(left, right, out bool result))
                    return result;
                throw new NotImplementedException($"Types has no comparison yet or never (different type)");
            }
            if (left is Fix32 fix)
                return fix == (Fix32)right;
            if (left is int i)
                return i == (int)right;
            if (left is float f)
                return f == (float)right;
            if (left is bool b)
                return b == (bool)right;
            throw new NotImplementedException($"Types has no comparison yet or never (same type)");
        }

        private static bool __eq__base(object left, object right, out bool result)
        {
            if (left is int leftI && right is Fix32 right32)
            {
                result = leftI.ToFix32() == right32;
                return true;
            }
            if (left is Fix32 left32 && right is int rightI)
            {
                result = left32 == rightI.ToFix32();
                return true;
            }
            if (left is float leftF && right is Fix32 right32_)
            {
                result = leftF.ToFix32() == right32_;
                return true;
            }
            if (left is Fix32 left32_ && right is float rightF)
            {
                result = left32_ == rightF.ToFix32();
                return true;
            }
            result = false;
            return false;
        }

        internal static bool __ne__(object left, object right)
        {
            if (left is null && right is null)
                return false;
            if (left is null || right is null)
                return true;
            if (left.GetType() != right.GetType())
            {
                if (__ne__base(left, right, out bool result))
                    return result;
                throw new NotImplementedException($"Types has no comparison yet or never (different type)");
            }
            if (left is Fix32 fix)
                return fix != (Fix32)right;
            if (left is int i)
                return i != (int)right;
            if (left is float f)
                return f != (float)right;
            if (left is bool b)
                return b != (bool)right;
            throw new NotImplementedException($"Types has no comparison yet or never (same type)");
        }

        private static bool __ne__base(object left, object right, out bool result)
        {
            if (left is int leftI && right is Fix32 right32)
            {
                result = leftI.ToFix32() != right32;
                return true;
            }
            if (left is Fix32 left32 && right is int rightI)
            {
                result = left32 != rightI.ToFix32();
                return true;
            }
            if (left is float leftF && right is Fix32 right32_)
            {
                result = leftF.ToFix32() != right32_;
                return true;
            }
            if (left is Fix32 left32_ && right is float rightF)
            {
                result = left32_ != rightF.ToFix32();
                return true;
            }
            result = false;
            return false;
        }

        internal static bool __ge__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot compare null values");
            if (left.GetType() != right.GetType())
            {
                if (__ge__base(left, right, out bool result))
                    return result;
                throw new NotImplementedException($"Types has no comparison yet or never (different type)");
            }
            if (left is Fix32 fix)
                return fix >= (Fix32)right;
            if (left is int i)
                return i >= (int)right;
            if (left is float f)
                return f >= (float)right;
            if (left is bool b)
                return true;
            throw new NotImplementedException($"Types has no comparison yet or never (same type)");
        }

        private static bool __ge__base(object left, object right, out bool result)
        {
            if (left is int leftI && right is Fix32 right32)
            {
                result = leftI.ToFix32() >= right32;
                return true;
            }
            if (left is Fix32 left32 && right is int rightI)
            {
                result = left32 >= rightI.ToFix32();
                return true;
            }
            if (left is float leftF && right is Fix32 right32_)
            {
                result = leftF.ToFix32() >= right32_;
                return true;
            }
            if (left is Fix32 left32_ && right is float rightF)
            {
                result = left32_ >= rightF.ToFix32();
                return true;
            }
            result = false;
            return false;
        }

        internal static bool __gt__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot compare null values");
            if (left.GetType() != right.GetType())
            {
                if (__gt__base(left, right, out bool result))
                    return result;
                throw new NotImplementedException($"Types has no comparison yet or never (different type)");
            }
            if (left is Fix32 fix)
                return fix > (Fix32)right;
            if (left is int i)
                return i > (int)right;
            if (left is float f)
                return f > (float)right;
            if (left is bool b)
                return b == true && (bool)right == false;
            throw new NotImplementedException($"Types has no comparison yet or never (same type)");
        }

        private static bool __gt__base(object left, object right, out bool result)
        {
            if (left is int leftI && right is Fix32 right32)
            {
                result = leftI.ToFix32() > right32;
                return true;
            }
            if (left is Fix32 left32 && right is int rightI)
            {
                result = left32 > rightI.ToFix32();
                return true;
            }
            if (left is float leftF && right is Fix32 right32_)
            {
                result = leftF.ToFix32() > right32_;
                return true;
            }
            if (left is Fix32 left32_ && right is float rightF)
            {
                result = left32_ > rightF.ToFix32();
                return true;
            }
            result = false;
            return false;
        }

        internal static bool __le__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot compare null values");
            if (left.GetType() != right.GetType())
            {
                if (__le__base(left, right, out bool result))
                    return result;
                throw new NotImplementedException($"Types has no comparison yet or never (different type)");
            }
            if (left is Fix32 fix)
                return fix <= (Fix32)right;
            if (left is int i)
                return i <= (int)right;
            if (left is float f)
                return f <= (float)right;
            if (left is bool b)
                return true;
            throw new NotImplementedException($"Types has no comparison yet or never (same type)");
        }

        private static bool __le__base(object left, object right, out bool result)
        {
            if (left is int leftI && right is Fix32 right32)
            {
                result = leftI.ToFix32() <= right32;
                return true;
            }
            if (left is Fix32 left32 && right is int rightI)
            {
                result = left32 <= rightI.ToFix32();
                return true;
            }
            if (left is float leftF && right is Fix32 right32_)
            {
                result = leftF.ToFix32() <= right32_;
                return true;
            }
            if (left is Fix32 left32_ && right is float rightF)
            {
                result = left32_ <= rightF.ToFix32();
                return true;
            }
            result = false;
            return false;
        }

        internal static bool __lt__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot compare null values");
            if (left.GetType() != right.GetType())
            {
                if (__lt__base(left, right, out bool result))
                    return result;
                throw new NotImplementedException($"Types has no comparison yet or never (different type)");
            }
            if (left is Fix32 fix)
                return fix < (Fix32)right;
            if (left is int i)
                return i < (int)right;
            if (left is float f)
                return f < (float)right;
            if (left is bool b)
                return b == false && (bool)right == true;
            throw new NotImplementedException($"Types has no comparison yet or never (same type)");
        }

        private static bool __lt__base(object left, object right, out bool result)
        {
            if (left is int leftI && right is Fix32 right32)
            {
                result = leftI.ToFix32() < right32;
                return true;
            }
            if (left is Fix32 left32 && right is int rightI)
            {
                result = left32 < rightI.ToFix32();
                return true;
            }
            if (left is float leftF && right is Fix32 right32_)
            {
                result = leftF.ToFix32() < right32_;
                return true;
            }
            if (left is Fix32 left32_ && right is float rightF)
            {
                result = left32_ < rightF.ToFix32();
                return true;
            }
            result = false;
            return false;
        }

        internal static object __range__(object left, Range range)
        {
            throw new NotImplementedException();
        }

        internal static object __index__(object left, object right)
        {
            throw new NotImplementedException();
        }

        internal static void __setitem__(object target, object index, object value)
        {
            if (target is List<object> list)
            {
                list[__int__(index)] = value;
                return;
            }
            if (target is IDictionary<string, object> dict)
            {
                dict[__str__(index)] = value;
            }
            throw new NotImplementedException();
        }

        internal static object __getitem__(object target, object index)
        {
            if (target is List<object> list)
            {
                return list[__int__(index)];
            }
            if (target is IDictionary<string, object> dict)
            {
                return dict.TryGetValue(__str__(index), out object value) ? value : null;
            }
            throw new NotImplementedException();
        }

        internal static bool __contains__(object target, object key)
        {
            if (target is null) throw new NullReferenceException("Target is None");
            if (target is IDictionary<string, object> dict)
                return dict.ContainsKey(__str__(key));
            if (target is List<object> list)
                return list.Contains(key);
            throw new NotImplementedException();
        }

        internal static object __invert__(object v)
        {
            throw new NotImplementedException();
        }

        internal static object __mul__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot multiply null values");
            if (left.GetType() != right.GetType())
                throw new NotImplementedException($"Types has no multiply yet or never (different type)");
            if (left is Fix32 fix)
                return fix * (Fix32)right;
            if (left is int i)
                return i * (int)right;
            if (left is float f)
                return f * (float)right;
            throw new NotImplementedException($"Types has no multiply yet or never (same type)");
        }

        internal static object __div__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot divide null values");
            if (left.GetType() != right.GetType())
                throw new NotImplementedException($"Types has no divide yet or never (different type)");
            if (left is Fix32 fix)
                return fix / (Fix32)right;
            if (left is int i)
                return i / (int)right;
            if (left is float f)
                return f / (float)right;
            throw new NotImplementedException($"Types has no divide yet or never (same type)");
        }

        internal static object __divint__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot divide null values");
            if (left.GetType() != right.GetType())
                throw new NotImplementedException($"Types has no multiply yet or never (different type)");
            if (left is Fix32 fix)
                return fix / (Fix32)right;
            if (left is int i)
                return i / (int)right;
            if (left is float f)
                return f / (float)right;
            throw new NotImplementedException($"Types has no divide yet or never (same type)");
        }

        internal static object __mod__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot divide null values");
            if (left.GetType() != right.GetType())
                throw new NotImplementedException($"Types has no divide yet or never (different type)");
            if (left is Fix32 fix)
                return fix % (Fix32)right;
            if (left is int i)
                return i % (int)right;
            if (left is float f)
                return f % (float)right;
            throw new NotImplementedException($"Types has no divide yet or never (same type)");
        }

        internal static bool __not__(object v)
        {
            if (v is bool b)
                return !b;
            throw new NotImplementedException();
        }

        internal static Fix32 __pow__(object left, object right)
        {
            Fix32 fixLeft = __fix__(left);
            Fix32 fixRight = __fix__(right);
            // todo
            return fixLeft.Pow(fixRight);
        }

        internal static object __lshift__(object left, object right)
        {
            if (left is null && right is null)
                return 0;
            return __int__(left) << __int__(right);
        }

        internal static object __rshift__(object left, object right)
        {
            if (left is null && right is null)
                return 0;
            return __int__(left) >> __int__(right);
        }

        internal static object __add__(object left, object right)
        {
            if (left is null || right is null)
                throw new NotImplementedException($"Cannot divide null values");
            if (left.GetType() != right.GetType())
                throw new NotImplementedException($"Types has no divide yet or never (different type)");
            if (left is Fix32 fix)
                return fix + (Fix32)right;
            if (left is int i)
                return i + (int)right;
            if (left is float f)
                return f + (float)right;
            throw new NotImplementedException($"Types has no divide yet or never (same type)");
        }
    }
}