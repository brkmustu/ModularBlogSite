ModularBlogSite

Öncelikler herkese merhabalar.

Bu uygulama aslýnda bir anlamda da kendi geliþtirdiðim (henüz emekleme aþamasýnda olsada :)) bir þablon mimarisi üzerine kurulmuþtur;
https://github.com/brkmustu/ModularTemplate

Bu uygulama ile en basit haliyle kullanýcý kayýt ve yönetim için temel özellikleri barýndýran bir kullanýcý üyelik yönetim sistemi amaçlanmýþtýr.

Uygulamadaki asýl amaç mikroservis mimarisi ile bu kurguyu yakalamaya çalýþmaktýr. Bu benim ilk örneðim olduðu için bir çok yerde kullanýlmamasý gereken þeyler kullanmýþ olabilirim :).
Yine de elimden geldiðince güzel bir yapý kurgulamaya çalýþtým.

Uygulamanýn ön planýnda bir apigateway bulunmaktadýr.

Arkada iki adet mikroservis bulunuyor. Biri "Management", diðeri "UserPortal" olmak üzere.

"Management" mikroservisi þu 3 temel apiyi hedef almaktadýr;

	- UserActivationStatusCommand => kullanýcýyý aktife yada pasife çekmeye yardýmcý olur (kullanýcý üstündeki aktif pasif bilgisini günceller)
	- UserChangeStatusCommand => kullanýcýnýn statu bilgisini güncellemeye yardýmcý olur (statü bilgisi aktiflikten daha detaylý bir bilgidir. onay bekliyor, reddedildi vs)
	- GetUserListQuery => kullanýcý listesini çekmek için kullanýlacak olan api servisidir, paging yapýsý mevcuttur.

"UserPortal" mikroservisi þu 3 temel apiyi hedef almaktadýr;

	- UserRegistrationCommand => kullanýcý kayýt iþlemleri için bu apiyi kullanabilirsiniz,
	- UserUpdateProfileCommand => kullanýcý güncelleme iþlemleri için bu api hazýrlanmýþtýr,
	- UserLoginQuery => login iþlemi için bu api hazýrlanmýþtýr.

Uygulamada Jwt token bazlý authentication yapýsý kurulmuþtur. Herhangi bir serviste yetki gerektiren bir api'ye istek atabilmek için,
öncelikle "UserPortal" mikroservisi altýndaki "UserLoginQuery" apisi yardýmý ile token almak gerekmektedir. Bearer token schemasý kullanýlmýþtýr.

Uygulamada hali hazýrda swagger entegrasyonu mevcuttur ve uygulama docker compose ile lokalinizde ayaða kaldýrabileceðiniz vaziyette olduðundan,
swagger üzerinden ilgili api servislerine ve modellerine eriþebilirsiniz.

Uygulama için örnek flow aþaðýdaki gibidir;

https://github.com/brkmustu/ModularBlogSite/blob/master/sample_flow.png

Þimdilik bu kadar :)
