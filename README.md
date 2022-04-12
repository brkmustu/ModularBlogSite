ModularBlogSite

�ncelikler herkese merhabalar.

Bu uygulama asl�nda bir anlamda da kendi geli�tirdi�im (hen�z emekleme a�amas�nda olsada :)) bir �ablon mimarisi �zerine kurulmu�tur;
https://github.com/brkmustu/ModularTemplate

Bu uygulama ile en basit haliyle kullan�c� kay�t ve y�netim i�in temel �zellikleri bar�nd�ran bir kullan�c� �yelik y�netim sistemi ama�lanm��t�r.

Uygulamadaki as�l ama� mikroservis mimarisi ile bu kurguyu yakalamaya �al��makt�r. Bu benim ilk �rne�im oldu�u i�in bir �ok yerde kullan�lmamas� gereken �eyler kullanm�� olabilirim :).
Yine de elimden geldi�ince g�zel bir yap� kurgulamaya �al��t�m.

Uygulaman�n �n plan�nda bir apigateway bulunmaktad�r.

Arkada iki adet mikroservis bulunuyor. Biri "Management", di�eri "UserPortal" olmak �zere.

"Management" mikroservisi �u 3 temel apiyi hedef almaktad�r;

	- UserActivationStatusCommand => kullan�c�y� aktife yada pasife �ekmeye yard�mc� olur (kullan�c� �st�ndeki aktif pasif bilgisini g�nceller)
	- UserChangeStatusCommand => kullan�c�n�n statu bilgisini g�ncellemeye yard�mc� olur (stat� bilgisi aktiflikten daha detayl� bir bilgidir. onay bekliyor, reddedildi vs)
	- GetUserListQuery => kullan�c� listesini �ekmek i�in kullan�lacak olan api servisidir, paging yap�s� mevcuttur.

"UserPortal" mikroservisi �u 3 temel apiyi hedef almaktad�r;

	- UserRegistrationCommand => kullan�c� kay�t i�lemleri i�in bu apiyi kullanabilirsiniz,
	- UserUpdateProfileCommand => kullan�c� g�ncelleme i�lemleri i�in bu api haz�rlanm��t�r,
	- UserLoginQuery => login i�lemi i�in bu api haz�rlanm��t�r.

Uygulamada Jwt token bazl� authentication yap�s� kurulmu�tur. Herhangi bir serviste yetki gerektiren bir api'ye istek atabilmek i�in,
�ncelikle "UserPortal" mikroservisi alt�ndaki "UserLoginQuery" apisi yard�m� ile token almak gerekmektedir. Bearer token schemas� kullan�lm��t�r.

Uygulamada hali haz�rda swagger entegrasyonu mevcuttur ve uygulama docker compose ile lokalinizde aya�a kald�rabilece�iniz vaziyette oldu�undan,
swagger �zerinden ilgili api servislerine ve modellerine eri�ebilirsiniz.

Uygulama i�in �rnek flow a�a��daki gibidir;

https://github.com/brkmustu/ModularBlogSite/blob/master/sample_flow.png

�imdilik bu kadar :)
