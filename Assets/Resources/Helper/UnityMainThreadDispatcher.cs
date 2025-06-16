using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using System;

/// Author: Pim de Witte (pimdewitte.com) and contributors, https://github.com/PimDeWitte/UnityMainThreadDispatcher
/// 
/// @brief 메인 스레드에서 실행되어야 하는 작업을 안전하게 큐잉하고 실행할 수 있도록 해주는 유틸리티 클래스.
/// @detail Firebase 등의 비동기 이벤트를 메인 스레드에서 처리할 때 유용함.
public class UnityMainThreadDispatcher : MonoBehaviour {

	/// @brief 메인 스레드에서 실행할 액션을 저장하는 큐
	private static readonly Queue<Action> _executionQueue = new Queue<Action>();

	/// @brief 매 프레임 Update 시 큐에 쌓인 작업을 하나씩 실행
	public void Update(){
		lock (_executionQueue){
			while (_executionQueue.Count > 0){
				_executionQueue.Dequeue().Invoke();
			}
		}
	}
	
	/// @brief IEnumerator를 받아 StartCoroutine으로 실행할 수 있도록 큐에 등록
	/// @param[action] 메인 스레드에서 실행할 IEnumerator 함수
	public void Enqueue(IEnumerator action){
		lock (_executionQueue){
			_executionQueue.Enqueue(() =>
			{
				StartCoroutine(action);
			});
		}
	}

	/// @brief 일반 Action을 받아 메인 스레드에서 실행되도록 큐에 등록
	/// @param[action] 메인 스레드에서 실행할 함수
	public void Enqueue(Action action){
		Enqueue(ActionWrapper(action));
	}

	/// @brief 비동기 방식으로 메인 스레드에서 실행될 Action을 등록하고, 해당 작업이 완료되었는지 확인 가능한 Task를 반환
	/// @param[action] 메인 스레드에서 실행할 함수
	/// @param[out] 실행 완료 시 완료되는 Task
	public Task EnqueueAsync(Action action){
		var tcs = new TaskCompletionSource<bool>();

		void WrappedAction(){
			try
			{
				action();
				tcs.TrySetResult(true);
			}
			catch (Exception ex)
			{
				tcs.TrySetException(ex);
			}
		}

		Enqueue(ActionWrapper(WrappedAction));
		return tcs.Task;
	}

	/// @brief Action을 IEnumerator로 감싸서 코루틴으로 실행할 수 있도록 변환
	IEnumerator ActionWrapper(Action a){
		a();
		yield return null;
	}

	//싱글톤 아오 싱글톤 싫은데
	private static UnityMainThreadDispatcher _instance = null;

	/// @brief 인스턴스 존재 여부 확인
	public static bool Exists(){
		return _instance != null;
	}

	/// @brief 인스턴스 반환, 존재하지 않을 경우 예외 발생
	public static UnityMainThreadDispatcher Instance(){
		if (!Exists())
		{
			throw new Exception("UnityMainThreadDispatcher could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
		}
		return _instance;
	}

	/// @brief 최초 생성 시 인스턴스 설정 및 씬 전환 시 파괴 방지
	void Awake() {
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad(this.gameObject);
		}
	}
	/// @brief 싱글톤 제거
	void OnDestroy(){
		_instance = null;
	}

}