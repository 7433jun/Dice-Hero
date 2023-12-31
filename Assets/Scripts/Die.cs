/**
 * Copyright (c) 2010-2015, WyrmTale Games and Game Components
 * All rights reserved.
 * http://www.wyrmtale.com
 *
 * THIS SOFTWARE IS PROVIDED BY WYRMTALE GAMES AND GAME COMPONENTS 'AS IS' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL WYRMTALE GAMES AND GAME COMPONENTS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR 
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */  
using UnityEngine;
using System.Collections;

/// <summary>
/// Die base class to determine if a die is rolling and to calculate it's current value
/// </summary>
public class Die : MonoBehaviour {

    //------------------------------------------------------------------------------------------------------------------------------
    // public attributes
    //------------------------------------------------------------------------------------------------------------------------------

    // current value, 0 is undetermined (die is rolling) or invalid.
    public int value = 0;

    // 공격가능 여부
    public bool useable = true;
    // 본래 색
    public Color color = Color.red;

	//------------------------------------------------------------------------------------------------------------------------------
	// protected and private attributes
	//------------------------------------------------------------------------------------------------------------------------------	
	
	// normalized (hit)vector from die center to upper side in local space is used to determine what side of a specific die is up/down = value
    protected Vector3 localHitNormalized;
	// hitVector check margin
    protected float validMargin = 0.01F;

    // localhit 이 0일때 다시 굴리고 나서 대기하게하는 플래그
    bool reRollFlag = true;

    // true is die is still rolling
    public bool rolling
    {
        get
        {
            return !(GetComponent<Rigidbody>().velocity.sqrMagnitude < .1F && GetComponent<Rigidbody>().angularVelocity.sqrMagnitude < .1F);
        }
    }

	// calculate the normalized hit vector and should always return true
    protected bool localHit
    {
        get
        {
			// create a Ray from straight above this Die , moving downwards
            Ray ray = new Ray(transform.position + (new Vector3(0, 2, 0) * transform.localScale.magnitude), Vector3.up * -1);
            RaycastHit hit = new RaycastHit();
			// cast the ray and validate it against this die's collider
            if (GetComponent<Collider>().Raycast(ray, out hit, 3 * transform.localScale.magnitude))
            {
				// we got a hit so we determine the local normalized vector from the die center to the face that was hit.
				// because we are using local space, each die side will have its own local hit vector coordinates that will always be the same.
                localHitNormalized = transform.InverseTransformPoint(hit.point.x, hit.point.y, hit.point.z).normalized;
                return true;
            }
			// in theory we should not get at this position!
            return false;
        }
    }

	// calculate this die's value
    void GetValue()
    {
		// value = 0 -> undetermined or invalid
        value = 0;
        float delta = 1;
		// start with side 1 going up.
        int side = 1;
        Vector3 testHitVector;
		// check all sides of this die, the side that has a valid hitVector and smallest x,y,z delta (if more sides are valid) will be the closest and this die's value
        do
        {
			// get testHitVector from current side, HitVector is a overriden method in the dieType specific Die subclass
			// eacht dieType subclass will expose all hitVectors for its sides,
            testHitVector = HitVector(side);
            if (testHitVector != Vector3.zero)
            {
				// this side has a hitVector so validate the x,y and z value against the local normalized hitVector using the margin.
                if (valid(localHitNormalized.x, testHitVector.x) &&
                    valid(localHitNormalized.y, testHitVector.y) &&
                    valid(localHitNormalized.z, testHitVector.z))
                {
					// this side is valid within the margin, check the x,y, and z delta to see if we can set this side as this die's value
					// if more than one side is within the margin (especially with d10, d12, d20 ) we have to use the closest as the right side
                    float nDelta = Mathf.Abs(localHitNormalized.x - testHitVector.x) + Mathf.Abs(localHitNormalized.y - testHitVector.y) + Mathf.Abs(localHitNormalized.z - testHitVector.z);
                    if (nDelta < delta)
                    {
                        value = side;
                        delta = nDelta;
                    }
                }
            }
			// increment side
            side++;
			// if we got a Vector.zero as the testHitVector we have checked all sides of this die
        } while (testHitVector != Vector3.zero);
    }

    void FixedUpdate()
    {
        // 구르지는 않지만 값이 나오지 않았을 경우
        if (!rolling && value == 0)
        {
            if (reRollFlag)
            {
                reRollFlag = false;
                StartCoroutine(CheckLocalHit());
            }
        }

		// determine the value is the die is not rolling
        if (!rolling && localHit)
        {
            GetValue();
        }
    }


	// validate a test value against a value within a specific margin.
    protected bool valid(float t, float v)
    {
        if (t > (v - validMargin) && t < (v + validMargin))
            return true;
        else
            return false;
    }

	// virtual  method that to get a die side hitVector.
	// this has to be overridden in the dieType specific subclass
    protected virtual Vector3 HitVector(int side)
    {
        return Vector3.zero;
    }
	
    // 잠시 대기 후 계속 value가 0이면 다시 굴림
    IEnumerator CheckLocalHit()
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if(!rolling && value == 0)
        {
            yield return new WaitForSecondsRealtime(1.0f);

            if(!rolling && value == 0)
            {
                DiceManager.Roll(this.gameObject);
            }
        }
        reRollFlag = true;
    }
}
