package com.hw.android.schdeuler

import java.math.BigInteger
import java.security.MessageDigest

//사용하지 않음(사용이 필요할 때를 위해서 보관)
class EncryptionManager {

    fun EncryptionPassword(logindata : String) : String
    {
        var result = MessageDigest.getInstance("SHA-256").let {
            var x = logindata.toByteArray()
            it.update(x)
            String.format("%064x", BigInteger(1, it.digest()))
        }

        return result;
    }
}