using System.Collections.Generic;
using Unity.Entities;

namespace ComponentGroupFilterTest
{
    struct Data : IComponentData { public int Value; }
    struct SharedData1 : ISharedComponentData { public int Value; }
    struct SharedData2 : ISharedComponentData { public int Value; }

    sealed class SpawnerSystem : ComponentSystem
    {
        bool _done;

        static bool RandomTF() { return UnityEngine.Random.value < 0.5f; }
        static int Random3() { return UnityEngine.Random.Range(0, 3); }

        protected override void OnUpdate()
        {
            if (_done) return;

            var dataArray = new [] {
                new Data { Value = 1 },
                new Data { Value = 2 },
                new Data { Value = 3 }
            };

            var sharedData1Array = new [] {
                new SharedData1 { Value = 1 },
                new SharedData1 { Value = 2 },
                new SharedData1 { Value = 3 }
            };

            var sharedData2Array = new [] {
                new SharedData2 { Value = 1 },
                new SharedData2 { Value = 2 },
                new SharedData2 { Value = 3 }
            };

            for (var i = 0; i < 1000; i++)
            {
                var entity = EntityManager.CreateEntity();
                EntityManager.AddComponentData(entity, dataArray[Random3()]);
                if (RandomTF()) EntityManager.AddSharedComponentData(entity, sharedData1Array[Random3()]);
                if (RandomTF()) EntityManager.AddSharedComponentData(entity, sharedData2Array[Random3()]);
            }

            _done = true;
        }
    }

    sealed class FilterSystem : ComponentSystem
    {
        bool _done;

        List<SharedData1> _sharedData1Array = new List<SharedData1>();
        List<SharedData2> _sharedData2Array = new List<SharedData2>();
        ComponentGroup _group;

        protected override void OnCreateManager(int capacity)
        {
            _group = GetComponentGroup(
                typeof(Data), typeof(SharedData1), typeof(SharedData2)
            );
        }

        protected override void OnUpdate()
        {
            if (_done) return;

            EntityManager.GetAllUniqueSharedComponentDatas(_sharedData1Array);
            EntityManager.GetAllUniqueSharedComponentDatas(_sharedData2Array);

            var sum = 0;

            foreach (var data1 in _sharedData1Array)
            {
                foreach (var data2 in _sharedData2Array)
                {
                    _group.SetFilter(data1, data2);

                    UnityEngine.Debug.Log(
                        "Data1 = " + data1.Value +
                        ", Data2 = " + data2.Value +
                        ", Length = " + _group.CalculateLength()
                    );

                    var dataArray = _group.GetComponentDataArray<Data>();
                    for (var i = 0; i < dataArray.Length; i++)
                        sum += dataArray[i].Value * data1.Value * data2.Value;
                }
            }

            _done = sum > 0;
        }
    }
}
