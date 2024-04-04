/*
 *  This is from the Meta SDK and it has been modified to fit our project.
 */
/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Oculus.Interaction.Input;
using UnityEngine;

#pragma warning disable CS0414 // 원본 API를 수정한 파일이므로 미사용 변수 경고를 표시하지 않길 원합니다.

namespace Oculus.Interaction.PoseDetection
{
    /// <summary>
    /// <see cref="ColliderContainsHandJointActiveState"/>의 변형으로, 해당 클래스와 달리
    /// Exit Collider 없이 Entry Collider만으로 활성화 상태 여부를 지속적으로 업데이트합니다. 
    /// </summary>

    public class ColliderContainsHandJointActiveStateCustom : MonoBehaviour, IActiveState
    {
        [SerializeField, Interface(typeof(IHand))]
        private UnityEngine.Object _hand;
        private IHand Hand;

        [SerializeField]
        private Collider _collider;

        [SerializeField]
        private HandJointId _jointToTest = HandJointId.HandWristRoot;

        public bool Active { get; private set; }

        private bool _active = false;

        protected virtual void Awake()
        {
            Hand = _hand as IHand;
            Active = false;
        }

        protected virtual void Start()
        {
            this.AssertField(Hand, nameof(Hand));
            this.AssertField(_collider, nameof(_collider));
        }

        protected virtual void Update()
        {
            if (Hand.GetJointPose(_jointToTest, out Pose jointPose))
            {
                Active = Collisions.IsPointWithinCollider(jointPose.position, _collider);
            }
            else
            {
                Active = false;
            }
        }
        
        #region Inject

        public void InjectAllColliderContainsHandJointActiveState(IHand hand, Collider collider, HandJointId jointToTest)
        {
            InjectHand(hand);
            InjectCollider(collider);
            InjectJointToTest(jointToTest);
        }

        public void InjectHand(IHand hand)
        {
            _hand = hand as UnityEngine.Object;
            Hand = hand;
        }

        public void InjectCollider(Collider collider)
        {
            _collider = collider;
        }

        public void InjectJointToTest(HandJointId jointToTest)
        {
            _jointToTest = jointToTest;
        }

        #endregion
    }
}
