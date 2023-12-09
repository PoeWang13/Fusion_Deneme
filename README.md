# Fusion Deneme
Photon Fusionu öğrenmek için yaptığım basit bir projedir. Agar.IO benzeridir ama mayın bırakma, roket fırlatma gibi özelliklerde ekledim. Player rengi şimdilik randomdur.

# Input
Player karakteri mouse pozisyonu yönünde hareket eder. M ile mayın bırakır. R ile roket fırlatılır.

# Mayın
M ile mayın bırakır. Fiyatı 2 birimdir. 5 damage verir. Herkese etki eder

# Roket
R ile roket bırakır. Fiyatı 5 birimdir. 10 damage verir. Herkese etki eder

# Player Name
İster yeni player name belirleyerek veya isterseniz eskiden kullandığınız bir isim ile oyuna başlarsınız.

# Lobby
Yeni bir oyun kurmak isterseniz Lobby Name inputuna lobby isminizi girmeniz gerekir. İsterseniz lobby'nize password input kısmından bir password belirleyebilirsiniz. Diğer oyuncular için lobby'niz gözükmeye başlayacakıtr. Eğer bir password belirlemişseniz lobbynizde password inputu gösterilecektir.
Lobby'ler şimdilik 2 kişilik olarak açılıyor ama isterseniz 
Network_Runner_Manager scripti içinde

    public async void CreateSession(string sessionName)
    {
        NetworkSceneManagerDefault networkSceneManager = networkRunner.GetComponent<NetworkSceneManagerDefault>();
        if (networkSceneManager == null)
        {
            networkSceneManager = networkRunner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }
        await networkRunner.StartGame(new StartGameArgs
        {
            GameMode = GameMode.AutoHostOrClient,
            PlayerCount = 2,
            SessionName = sessionName,
            SceneManager = networkSceneManager
        });
        Game_Manager.Instance.FindRunner();
    }
    fonksiyonundaki 'PlayerCount = 2' satırındaki 2 rakamını değiştirerek istediğiniz rakamı belirleyebilirsiniz.

# Lobby'e giriş
Başkasının oyununa bağlanmak istiyorsanız, lobby listesindeki bir lobby'yi belirlemeniz ve JOIN butonuna tıklamanız yeterli. Eğer lobby'de yer varsa ve password doğruysa sizi oyuna bağlayacaktır.

# Öneri-şikayet
İstediğiniz gibi inceleyebilirsiniz. Öneri ve şikayetlerinizi 13yedecim13@gmail.com veya https://www.linkedin.com/in/h%C3%BCseyin-emre-can-0b2028162/ ile linkedin üzerinden ulaşabilirsiniz. 
