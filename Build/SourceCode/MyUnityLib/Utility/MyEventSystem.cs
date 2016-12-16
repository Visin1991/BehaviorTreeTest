using System;
using System.Collections;

public class MyEventSystem{

    public class SkillEventArgs : EventArgs {
        private int index;
        private float cooldownTime;
        public SkillEventArgs(int _index,float _cooldownTime) {
            index = _index;
            cooldownTime = _cooldownTime;
        }

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public float CooldownTIme {
            get { return cooldownTime; }
            set { cooldownTime = value; }
        }
    }

}
