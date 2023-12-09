using UnityEngine;

public class Fikirler : MonoBehaviour
{
    #region Genel RPC Networked stat fonksiyons Fusion interface
    // Public or inspector values
    // Private values
    // Network Values
    #endregion

    /*
     * Object.HasStateAuthority == Script serverda m� �al��t�r�l�yor?
     * Object.HasInputAuthority == Script localda m� �al��t�r�l�yor? ve bizim kontrol�m�zde. 
     * Mesela playerlar olur 1 tanesi bizim olur bunu belirlemek i�in kullan�l�r.
     * 
     * ushort size = 5; // Max = 65.535 oluyor.
     * float movementSpeed = (size / Mathf.Pow(size, 1.1f)) * 2;
     * size -> Objenin scale'ine referans ediyor. Size k�sm�na g�re b�y�kl�k ve k���kl�k ile h�z de�i�iyor.
     * 
     * public class XScript : MonoBehaviour
     * {
     * 
     * [Networked(OnChanged == nameof(OnSizeChanged))] // size de�i�ti�inde OnSizeChanged fonksiyonu otomatik aktif olur.
     * int size { get; set; } // Bu tip de�i�kenler = 3; �eklinde d�zeltilemez, property olmak zorunda.
     * void UpdateSize() { // Bir�eyler yap. }
     * 
     *
     * static void OnSizeChanged(Changed<XScript> changed) // size de�i�kenine ba�l� olabilmesi i�in static olmas� gerekir.
     * { 
     *  changed.Behaviour.UpdateSize(); // UpdateSize fonksiyonu �al��t�r�l�r.
     * }
     * }
     * NetworkSpawner scriptine INetworkRunnerCallbacks interface eklenecek.
     * public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef) // Player eklenince �a�r�l�r
     * {
     * if(runner.IsServer){ // Serverda m� �al���yoruz}
     * }
     * public void OnInput(NetworkRunner runner, NetworkInput networkInput) // Player Input yap�nca �a�r�l�r.
     * {
     * networkInput.Set();
     * }
     * 
     * private string nickName;
     * [Rpc(RpcSources.InputAuthority, Rpc.StateAuthority)] // Clientten server'a haber vermek i�in kullan�laan Attribut
     * private void RPC_JoinGame(string nickName, RpcInfo info = default) // RPC ile ba�lamal� fonksiyon
     * {
     * this.nickName=nickName; // serverda scriptin i�indeki nickName de�i�keni clientten gelen nickName ile de�i�tirildi.
     * } // Server'da XX idi clientta Emre yap�ld�. Serverdada Emre oldu.
     * 
     * GetComponent<NetworkRigidbody2D>().TeleportToPosition(Vector3.zero); // componentin oldu�u objeyi 
     * ilgili pozisyona an�nda g�nderir. 
     * 
     * Script MonoBehaviour de�il NetworkBehaviour veya SimulationBehaviour olmal�.
     * Bir objeyi spawn etmek i�in kullan�l�r. Serverda yap�l�rsa t�m clientta olur.
     * NetworkObject obj = Runner.Spawn(prefab, Vector.zero, Quaternion.identity, PlayerRef, Action);
     * PlayerRef player i�in referans, Action ise obj spawn yap�lmadan �nce �a�r�lacak Action fonksiyonu.
     * Bazen obj verilen yerde olu�turulmaz. Bu y�zden pozisyonu tekrardan setlenir.
     * obj.transform.position = Vector3.one;
     * 
     * public override void FixedUpdateNetwork() // Network'de ge�en fixedupdate'tir.
     * {
     * if(!Runner.HasStateAuthority) return; // Server de�ilse devam etme.
     * Obj'nin collider'�n� pasif yap�yoruzki kendi kendine �arp��m�� say�lmas�n.
     * obj.circleCollider2D.enable = false;
     * 
     * Obj'nin ekranda herhangi bir colliderla �arp���p �arp��mad���na bakar
     * Collider2D hitCollider = Runner.GetPhysicsScene2D().OverlapCircle(obj.transform.position, obj.transform.localScale.x / 2);
     * 
     * Obj'nin collider'�n� aktif yap�yoruzki ba�kalar�yla �arp��abilsin
     * obj.circleCollider2D.enable = true;
     * 
     * if(hitCollider != null) if(hitCollider.CompareTag("Tag�m"))
     * // obj bir�eye �arpm��sa ve �arp�lan obje 'Tag�m' tag�na sa�ipse bir�eyler yap.
     * }
     * 
     * TickTimer zamanlay�c� = TickTimer.CreateFromSeconds(Runner, 2); // Geriye do�ru sayan bir zamanlay�c� kurar.
     * if(zamanlay�c�.IsRunning) // Zamanlay�c� �al���yor mu
     * if(zamanlay�c�.Expired(Runner)) // Zamanlay�c� bitti mi
     * 
     * zamanlay�c� = TickTimer.None; // Zamanlay�c�y� bitir.
     * 
     */

    /* 
     * 
     */

}