Veritabanında bir değişiklik varsa ve geri alınması isteniyorsa loglama sistemi kullanılabilir. Kayıt sistemindeki kaydın kimliğini biliyorsanız, kimliği belirterek ``RestoreToLog(string id)`` işlevi ile kaydı geri yükleyebilirsiniz.
Kimliğinizden emin değilseniz, bu kimliğe sahip bir günlük olup olmadığını ``ExistsLog(string id)`` işleviyle öğrenebilirsiniz. <br>
İlk günlüğe dönmek istiyorsanız ``RestoreToFistLog()`` işlevini veya son kayda dönmek istiyorsanız ``RestoreToLastLog()`` işlevini kullanabilirsiniz.
