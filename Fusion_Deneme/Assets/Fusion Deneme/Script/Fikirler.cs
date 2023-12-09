using UnityEngine;

public class Fikirler : MonoBehaviour
{
    #region Genel RPC Networked stat fonksiyons Fusion interface
    // Public or inspector values
    // Private values
    // Network Values
    #endregion

    /*
     * Object.HasStateAuthority == Script serverda mý çalýþtýrýlýyor?
     * Object.HasInputAuthority == Script localda mý çalýþtýrýlýyor? ve bizim kontrolümüzde. 
     * Mesela playerlar olur 1 tanesi bizim olur bunu belirlemek için kullanýlýr.
     * 
     * ushort size = 5; // Max = 65.535 oluyor.
     * float movementSpeed = (size / Mathf.Pow(size, 1.1f)) * 2;
     * size -> Objenin scale'ine referans ediyor. Size kýsmýna göre büyüklük ve küçüklük ile hýz deðiþiyor.
     * 
     * public class XScript : MonoBehaviour
     * {
     * 
     * [Networked(OnChanged == nameof(OnSizeChanged))] // size deðiþtiðinde OnSizeChanged fonksiyonu otomatik aktif olur.
     * int size { get; set; } // Bu tip deðiþkenler = 3; þeklinde düzeltilemez, property olmak zorunda.
     * void UpdateSize() { // Birþeyler yap. }
     * 
     *
     * static void OnSizeChanged(Changed<XScript> changed) // size deðiþkenine baðlý olabilmesi için static olmasý gerekir.
     * { 
     *  changed.Behaviour.UpdateSize(); // UpdateSize fonksiyonu çalýþtýrýlýr.
     * }
     * }
     * NetworkSpawner scriptine INetworkRunnerCallbacks interface eklenecek.
     * public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef) // Player eklenince çaðrýlýr
     * {
     * if(runner.IsServer){ // Serverda mý çalýþýyoruz}
     * }
     * public void OnInput(NetworkRunner runner, NetworkInput networkInput) // Player Input yapýnca çaðrýlýr.
     * {
     * networkInput.Set();
     * }
     * 
     * private string nickName;
     * [Rpc(RpcSources.InputAuthority, Rpc.StateAuthority)] // Clientten server'a haber vermek için kullanýlaan Attribut
     * private void RPC_JoinGame(string nickName, RpcInfo info = default) // RPC ile baþlamalý fonksiyon
     * {
     * this.nickName=nickName; // serverda scriptin içindeki nickName deðiþkeni clientten gelen nickName ile deðiþtirildi.
     * } // Server'da XX idi clientta Emre yapýldý. Serverdada Emre oldu.
     * 
     * GetComponent<NetworkRigidbody2D>().TeleportToPosition(Vector3.zero); // componentin olduðu objeyi 
     * ilgili pozisyona anýnda gönderir. 
     * 
     * Script MonoBehaviour deðil NetworkBehaviour veya SimulationBehaviour olmalý.
     * Bir objeyi spawn etmek için kullanýlýr. Serverda yapýlýrsa tüm clientta olur.
     * NetworkObject obj = Runner.Spawn(prefab, Vector.zero, Quaternion.identity, PlayerRef, Action);
     * PlayerRef player için referans, Action ise obj spawn yapýlmadan önce çaðrýlacak Action fonksiyonu.
     * Bazen obj verilen yerde oluþturulmaz. Bu yüzden pozisyonu tekrardan setlenir.
     * obj.transform.position = Vector3.one;
     * 
     * public override void FixedUpdateNetwork() // Network'de geçen fixedupdate'tir.
     * {
     * if(!Runner.HasStateAuthority) return; // Server deðilse devam etme.
     * Obj'nin collider'ýný pasif yapýyoruzki kendi kendine çarpýþmýþ sayýlmasýn.
     * obj.circleCollider2D.enable = false;
     * 
     * Obj'nin ekranda herhangi bir colliderla çarpýþýp çarpýþmadýðýna bakar
     * Collider2D hitCollider = Runner.GetPhysicsScene2D().OverlapCircle(obj.transform.position, obj.transform.localScale.x / 2);
     * 
     * Obj'nin collider'ýný aktif yapýyoruzki baþkalarýyla çarpýþabilsin
     * obj.circleCollider2D.enable = true;
     * 
     * if(hitCollider != null) if(hitCollider.CompareTag("Tagým"))
     * // obj birþeye çarpmýþsa ve çarpýlan obje 'Tagým' tagýna saðipse birþeyler yap.
     * }
     * 
     * TickTimer zamanlayýcý = TickTimer.CreateFromSeconds(Runner, 2); // Geriye doðru sayan bir zamanlayýcý kurar.
     * if(zamanlayýcý.IsRunning) // Zamanlayýcý çalýþýyor mu
     * if(zamanlayýcý.Expired(Runner)) // Zamanlayýcý bitti mi
     * 
     * zamanlayýcý = TickTimer.None; // Zamanlayýcýyý bitir.
     * 
     */

    /* 
     * 
     */

}