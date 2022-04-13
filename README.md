ModularBlogSite

Öncelikler herkese merhabalar.

Bu uygulama aslında bir anlamda da kendi geliştirdiğim (henüz emekleme aşamasında olsada :)) bir şablon mimarisi üzerine kurulmuştur; https://github.com/brkmustu/ModularTemplate

Bu uygulama ile en basit haliyle kullanıcı kayıt ve yönetim için temel özellikleri barındıran bir kullanıcı üyelik yönetim sistemi amaçlanmıştır.

Uygulamadaki asıl amaç mikroservis mimarisi ile bu kurguyu yakalamaya çalışmaktır. Bu benim ilk örneğim olduğu için bir çok yerde kullanılmaması gereken şeyler kullanmış olabilirim :). Yine de elimden geldiğince güzel bir yapı kurgulamaya çalıştım.

Uygulamanın ön planında bir apigateway bulunmaktadır. Şimdilik docker üzerinde çalıştıramadım, ilk fırsatta bununla da ilgileneceğim.

Arkada iki adet mikroservis bulunuyor. Biri **"Management"**, diğeri **"UserPortal"** olmak üzere.

**"Management"** mikroservisi şu 3 temel apiyi hedef almaktadır;

- **"UserActivationStatusCommand"** => kullanıcıyı aktife yada pasife çekmeye yardımcı olur (kullanıcı üstündeki aktif pasif bilgisini günceller)
- **"UserChangeStatusCommand"** => kullanıcının statu bilgisini güncellemeye yardımcı olur (statü bilgisi aktiflikten daha detaylı bir bilgidir. onay bekliyor, reddedildi vs)
- **"GetUserListQuery"** => kullanıcı listesini çekmek için kullanılacak olan api servisidir, paging yapısı mevcuttur.

Bu uygulamayı lokalinizde ayağa kaldırdıktan sonra "http://localhost:5010/swagger/index.html" adresi üzerinden ilgili servislere erişebilirsiniz.

**"UserPortal"** mikroservisi şu 3 temel apiyi hedef almaktadır;

- **"UserRegistrationCommand"** => kullanıcı kayıt işlemleri için bu apiyi kullanabilirsiniz,
- **"UserUpdateProfileCommand"** => kullanıcı güncelleme işlemleri için bu api hazırlanmıştır,
- **"UserLoginQuery"** => login işlemi için bu api hazırlanmıştır.

Bu uygulamayı lokalinizde ayağa kaldırdıktan sonra "http://localhost:5020/swagger/index.html" adresi üzerinden ilgili servislere erişebilirsiniz.

Uygulamada Jwt token bazlı authentication yapısı kurulmuştur. Herhangi bir serviste yetki gerektiren bir api'ye istek atabilmek için, öncelikle "UserPortal" mikroservisi altındaki "UserLoginQuery" apisi yardımı ile token almak gerekmektedir. Bearer token scheması kullanılmıştır.

ApiGateway'i şimdilik docker üzerinde stabil çalışır hale getiremediğimden, lokalde çalışırken direkt servislerin kendi swagger UI'larını kullanarak çalıştırabilirsiniz. Tabi projeyi kendi lokalinizde ayağa kaldırmanızı kolaylaştırabilmek için powershell dosyalarıda ekledim. Bunlar;

- **"docker-compose-up.ps1"** => uygulamayı lokalinizde ayağa kaldırmanıza yardımcı olur ve tekraren çalıştırdığınızda mevcut image'leri kullanır.
- **"docker-compose-up-force-recreate.ps1"** >= uygulama üzerinde eğer bir geliştirme yaptıysanız, mevcut projelerin image'larını tekrar oluşturmaya zorlar (aslında bu daha çok uygulamayı geliştirenlere yardımcı olacak bir script).
- **"docker-compose-down.ps1"** => uygulamayı kendi lokalinizde denedikten sonra lokalinizden kaldırmak isterseniz bu powershell script'ini kullanabilirsiniz.

Uygulamada hali hazırda swagger entegrasyonu mevcuttur ve uygulama docker compose ile lokalinizde ayağa kaldırabileceğiniz vaziyette olduğundan, swagger üzerinden ilgili api servislerine ve modellerine erişebilirsiniz.

Uygulamayı ayağa kaldırdığınızda kendisi otomatik olarak database'lerini oluşturup, yetki tanımlarını database'lerine aktarabilmektedir. Tabi birde uygulama için **"admin"** kullanıcısı varsayılan olarak db'lere eklenmektedir. **"admin"** kullanıcısının şifresi : **"1qaz!2wsx"** olarak ayarlanmıştır. bu şekilde token alınıp, denemeler yapılabilir.

Uygulama için örnek flow aşağıdaki gibidir;

![Örnek Flow!](https://github.com/brkmustu/ModularBlogSite/blob/master/sample_flow.png "Örnek Flow")

Şimdilik bu kadar :)