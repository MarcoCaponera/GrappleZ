using GrappleZ_Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrappleZ_Player
{

    public class PlayerShoot : PlayerAbilityBase
    {
        public Camera playerCamera;
        public bool isShooting, readToShoot;
        bool allowTeset = true;

        public float shootingDelay = 2f;
        public int bulletsPerBrust = 3;
        public int burstBuletLeft;
        public float recoilIntesity;
        public float knockBackForce;


        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public float bulletSpeed = 30f;
        public float bulletLifetime = 3f;


        public enum ShootingMode
        {
            Single,
            Burst,
            Auto

        }
        public ShootingMode currentShootingMode;

        private void Awake()
        {
            readToShoot = true;
            burstBuletLeft = bulletsPerBrust;
        }

        void Fire()
        {

            readToShoot = false;

            Vector3 shootingDirection = CalculateRecoil().normalized;

            playerController.AddRigidBodyForce(-shootingDirection * knockBackForce, ForceMode.Impulse);

            GameObject bulllet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
            bulllet.transform.forward = shootingDirection;
            bulllet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized * bulletSpeed, ForceMode.Impulse);
            StartCoroutine(destroyBullet(bulllet, bulletLifetime));
            if (allowTeset)

            {

                Invoke("ResetShot", shootingDelay);
                allowTeset = false;
            }
            if (currentShootingMode == ShootingMode.Burst && burstBuletLeft > 1)
            {
                burstBuletLeft--;
                Invoke("Fire", shootingDelay);
            }

        }
        void ResetShot()
        {
            readToShoot = true;
            allowTeset = true;
        }
        public Vector3 CalculateRecoil()
        {

            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            RaycastHit hit;

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {

                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100);
            }
            Vector3 direction = targetPoint - bulletSpawn.position;
            float x = UnityEngine.Random.Range(-recoilIntesity, recoilIntesity);
            float y = UnityEngine.Random.Range(-recoilIntesity, recoilIntesity);
            return direction + new Vector3(x, y, 0);

        }
        IEnumerator destroyBullet(GameObject bullet, float timeTo)
        {
            yield return new WaitForSeconds(timeTo);
            Destroy(bullet);
        }

        public override void Init(PlayerController playerController, PlayerVisual playerVisual)
        {
            base.Init(playerController, playerVisual);
            InputManager.ManageShootSubscription(OnInputPerformed, true);
        }

        public override void OnInputEnabled()
        {
            
        }

        public override void OnInputDisabled()
        {
            
        }

        public override void StopAbility()
        {
            
        }

        private void OnInputPerformed(InputAction.CallbackContext context)
        {
            Fire();
        }
    }
}

