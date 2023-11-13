using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle
{
    public class PlayManager : MonoBehaviour
    {
        Scenario scenario;
        IEnumerator<bool> enumerator;

        private void Awake()
        {
            scenario = new Stage1();
            enumerator = scenario.GetEnumerator();
        }
        public void Update()
        {
            if (enumerator.MoveNext() == false)
            {
                // todo -> finish stage.
            }
        }
    }


    public class Stage1 : Scenario
    {
        UnityEngine.Transform a;
        UnityEngine.Transform b;
        UnityEngine.Transform c;

        public Stage1()
        {
            conditions.Add(() => Vector3.Distance(a.position, b.position) < 0.5f);
            actions.Add(() => c.position += Vector3.right * 30.0f);
        }
    }


    public abstract class Scenario : IEnumerable<bool>
    {
        public List<Func<bool>> conditions;
        public List<Action> actions;

        public IEnumerator<bool> GetEnumerator()
        {
            return Enumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Enumerator();
        }

        private IEnumerator<bool> Enumerator()
        {
            int index = 0;
            while (conditions.Count > 0)
            {
                if (conditions[index].Invoke())
                {
                    actions[index].Invoke();
                    index++;
                    yield return true;
                }
            }
            yield return false;
        }
    }
}