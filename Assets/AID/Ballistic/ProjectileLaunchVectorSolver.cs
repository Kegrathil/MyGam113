using UnityEngine;
using System.Collections;

//TODO
//	we could adjust for the radius of both objects, currently they are both just points
//	launcher init velocity
//	we could return all solutions not just the first
//	differing gravity settings

namespace AID
{
    /*
     * Utility class for finding the launch direction, time of flight and the predicted impact point for hiting a moving target with a projectile.
     * 
     * Note that impact position is where the target will be when the impact is predicted, this will almost always be off
     * 	due to the alg not knowing/caring how large the objects involved are.
     */
    [System.Serializable]
    public class ProjectileLaunchVectorSolver
    {

        //required input data
        public Vector3 launchPosition, targetInitialPosition, targetInitialVelocity;
        public float launchSpeed;
        public Vector3 gravity;
        public bool isTargetFalling, isProjectileFalling;   //these signify which of the objects are falling under the effect of gravity

        public bool CanHit { get; private set; }                //is a hit possible given previous data and solve
        public float TimeToImpact { get; private set; }     //how long till the impact will occur
        public Vector3 LaunchDirection { get; private set; }    //unit vector in the direction that projectile should be launched
        public Vector3 LaunchVelocity { get; private set; } //LaunchDirection*launchSpeed
        public Vector3 ImpactPosition { get; private set; } //Where does the impact occur?


        public ProjectileLaunchVectorSolver() { }

        public ProjectileLaunchVectorSolver(Vector3 targetInitialPos, Vector3 targetInitialVel,
                                            Vector3 launchFrom, float projectileSpeed)
        {
            SetAll(targetInitialPos, targetInitialVel, launchFrom, projectileSpeed, Vector3.zero, false, false);
        }


        public ProjectileLaunchVectorSolver(Vector3 targetInitialPos, Vector3 targetInitialVel,
                                            Vector3 launchFrom, float projectileSpeed,
                                            Vector3 grav,
                                            bool targetIsFalling, bool projectileIsFalling)
        {
            SetAll(targetInitialPos, targetInitialVel, launchFrom, projectileSpeed, grav, targetIsFalling, projectileIsFalling);
        }

        public void SetAll(Vector3 targetInitialPos, Vector3 targetInitialVel,
                           Vector3 launchFrom, float projectileSpeed,
                           Vector3 Gravity,
                           bool targetIsFalling, bool projectileIsFalling)
        {
            launchPosition = launchFrom;
            targetInitialPosition = targetInitialPos;
            targetInitialVelocity = targetInitialVel;
            launchSpeed = projectileSpeed;
            gravity = Gravity;
            isTargetFalling = targetIsFalling;
            isProjectileFalling = projectileIsFalling;
        }

        /*
         * Returns false if a hit is impossible, if true you can then use LaunchDirection or LaunchVelocity 
         */
        public bool Solve()
        {
            //if grav is the same then we ignore it and run linear linear as they are either not falling or falling at the same rate
            if (isProjectileFalling == isTargetFalling || Mathf.Approximately(gravity.sqrMagnitude, 0f))
            {
                TimeToImpact = CalcIntersectTime(targetInitialPosition, targetInitialVelocity, launchPosition, launchSpeed);
                CanHit = TimeToImpact != Mathf.Infinity;

                if (CanHit)
                {
                    ImpactPosition = targetInitialPosition + targetInitialVelocity * TimeToImpact;
                    LaunchDirection = (ImpactPosition - launchPosition).normalized;
                    LaunchVelocity = LaunchDirection * launchSpeed;
                }
            }
            else
            {
                //one is falling need full quartic
                float time = 0f;
                Vector3 hitPos = Vector3.zero, firVel = Vector3.zero;
                //if the target is falling not the bullet then we fake it as tho the projectile is being pulled away from gravity
                firVel = CalcShootVect(targetInitialPosition, targetInitialVelocity,
                                            launchPosition, launchSpeed,
                                            isTargetFalling ? -gravity : gravity, out hitPos, out time);

                TimeToImpact = time;
                CanHit = TimeToImpact != Mathf.Infinity;

                if (CanHit)
                {
                    ImpactPosition = hitPos;
                    LaunchVelocity = firVel;
                    LaunchDirection = LaunchVelocity.normalized;
                }
            }

            return CanHit;
        }

        /*
         * Sets all then runs solve
         */
        public bool Solve(Vector3 targetInitialPos, Vector3 targetInitialVel,
                          Vector3 launchFrom, float projectileSpeed,
                          Vector3 grav,
                          bool targetIsFalling, bool projectileIsFalling)
        {
            SetAll(targetInitialPos, targetInitialVel, launchFrom, projectileSpeed, grav, targetIsFalling, projectileIsFalling);
            return Solve();
        }

        /*
        * Calc time to intersect of linear moving target, given intial positions and known speed
        */
        public static float CalcIntersectTime(Vector3 enemyPos, Vector3 enemyVelocity, Vector3 firingPos, float bulletSpeed)
        {
            //using the expanding sphere vs growing line method
            //http://playtechs.blogspot.kr/2007/04/aiming-at-moving-target.html

            //make local, sphere is now at origin
            Vector3 dif = enemyPos - firingPos;

            //squared speed dif
            float a = bulletSpeed * bulletSpeed - Vector3.Dot(enemyVelocity, enemyVelocity);

            //two times dot of our vector and the escape vel
            float b = -2 * Vector3.Dot(enemyVelocity, dif);

            float c = -Vector3.Dot(dif, dif);

            QuadraticResult res = UTIL.QuadraticEquation(a, b, c);

            switch (res.numResults)
            {
                case 1:
                    if (res.r1 > 0)
                    {
                        return res.r1;
                    }
                    break;
                case 2:
                    float max = Mathf.Max(res.r1, res.r2);

                    if (max > 0)
                    {
                        return max;
                    }
                    break;
                default:
                    break;
            }
            return Mathf.Infinity;
        }
        /*
            determines the point at which a linear projectile will hit a linearly moving target
        */
        public static Vector3 CalcIntersectPoint(Vector3 enemyPos, Vector3 enemyVelocity, Vector3 firingPos, float bulletSpeed)
        {
            float dist = CalcIntersectTime(enemyPos, enemyVelocity, firingPos, bulletSpeed);

            if (dist != Mathf.Infinity)
            {
                return enemyPos + enemyVelocity * dist;
            }

            return Vector3.one * Mathf.Infinity;
        }


        /*
            Returns the direction to fire at a given linearly moving enemy with a fixed projectile velocity
        */
        public static Vector3 CalcIntersectVelocity(Vector3 enemyPos, Vector3 enemyVelocity, Vector3 firingPos, float bulletSpeed)
        {
            return CalcIntersectPoint(enemyPos, enemyVelocity, firingPos, bulletSpeed) - firingPos;
        }


        /*
            Returns the direction to fire at a given linearly moving enemy with a fixed projectile velocity
        */
        public static Vector3 CalcIntersectDirection(Vector3 enemyPos, Vector3 enemyVelocity, Vector3 firingPos, float bulletSpeed)
        {
            return CalcIntersectVelocity(enemyPos, enemyVelocity, firingPos, bulletSpeed).normalized;
        }



        /*Calculate aiming ideal rotation for firing a projectile at a potentially moving target (assumes pawn physics)
     IN:
     -projectileStart = world location where projectile is starting at
     -projectileSpeed = speed of the projectile being fired
     -grav = a vector describing the grav
     -target = the actual targetted ACTOR
     OUT:
     -dest: Location where the projectile will collide with target
     -returns vector describing direction for projectile to leave at

    orig from http://playtechs.blogspot.kr/2007/04/aiming-at-moving-target.html , http://wiki.beyondunreal.com/Legacy:Projectile_Aiming
    */
        static public Vector3 CalcShootVect(Vector3 targetPos, Vector3 targetVel, Vector3 projectileStart, float projectileSpeed, Vector3 grav, out Vector3 Dest, out float time)
        {
            QuarticResult Q;
            float best;//, speed2D, HitTime;
            Vector3 Pr;
            // int i;
            //  Vector3 HitNorm, HitLoc;
            Vector3 D; //EndLoc-projectileStart
            Vector3 V; //target.velocity

            D = targetPos - projectileStart;
            V = targetVel;

            //todo: add traces to check side walls?
            //note: walking velocity *should* factor in current slope
            best = 0;
            Q = UTIL.QuarticEquation(0.25f * Vector3.Dot(grav, grav),
                                      Vector3.Dot(-grav, V),
                                      Vector3.Dot(-grav, D) + Vector3.Dot(V, V) - projectileSpeed * projectileSpeed,
                                      2 * Vector3.Dot(V, D),
                                      Vector3.Dot(D, D));

            //get lowest res that is greater than 0
            best = Q.SmallestRealAboveZero();

            if (best <= 0)
            {   //projectile is out of range
                //Warning: Out of range adjustments assume grav is parallel to the z axis and pointed downward!!
                Pr.z = projectileSpeed / UTIL.SQRT2f; //determine z direction of firing
                best = -2 * Pr.z / grav.z;
                best += ((D.magnitude) - Pr.z * best) / projectileSpeed; //note p.z = 2D vsize(p)  (this assumes ball travels in a straight line after bounce)
                                                                         //now recalculate PR to handle velocity prediction (so ball at least partially moves in direction of player)
                Pr = D / best + V - 0.5f * grav * best;
                //now force maximum height again:
                Pr.z = 0;
                Pr = (projectileSpeed / UTIL.SQRT2f) * Pr.normalized;
                Pr.z = projectileSpeed / UTIL.SQRT2f; //maxmimum
                Dest = projectileStart + Pr * best + 0.5f * grav * best * best;
                time = Mathf.Infinity;
                return Pr;
            }

            Pr = (D / best + V - 0.5f * grav * best).normalized * projectileSpeed;

            Dest = projectileStart + Pr * best + 0.5f * grav * best * best;
            time = best;
            return Pr;
        }
    }
}