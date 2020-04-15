package com.hw.android.schdeuler

import android.app.Activity
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Toast
import kotlinx.android.synthetic.main.activity_login.*

class LoginActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        val mainIntent = Intent(this, MainActivity::class.java)
        val memberManaguer = MemberManager()

        //자동로그인 확인값 받아오기
        var saveCheck = getSharedPreferences("auto", Activity.MODE_PRIVATE).let {
            var check = it.getString("inputCheck", "0")
            check
        }

        //자동로그인을 하면
        if(saveCheck?.toInt()==1) {

            //저장된 ID, PW 받아옴
            var saveLoginData = getSharedPreferences("auto", Activity.MODE_PRIVATE).let {
                var loginData = LoginData()
                loginData.phone = it.getString("inputPhone", null).orEmpty()
                loginData.password = it.getString("inputPassword", null).orEmpty()
                loginData
            }

            //저장된 값으로 DB검색
            var getLoginData = memberManaguer.selectMemberData(saveLoginData)

            //DB에 값이 있는지 확인 및 비밀번호 확인
            if (getLoginData.password.equals(saveLoginData.password)) {
                MemberData.setMemberData(getLoginData)
                startActivity(mainIntent)
            }
        }

        //로그인버튼 클릭
        btnLogin.setOnClickListener {

            if(!(etPhone.text.trim().count() <= 11 && etPhone.text.trim().count() >= 10))
            {
                Toast.makeText(this, "아이디를 확인하세요",Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }
            if(etPassword.text.isBlank() || etPassword.text.isEmpty())
            {
                Toast.makeText(this, "비밀번호를 확인하세요",Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            //입력받은 데이터 loginData에 저장
            val loginData = LoginData().also {
                it.phone = etPhone.text.toString()
                it.password = etPassword.text.toString()        //비밀번호 암호화 코드 필요(CS프로그램의 암호화와 안드로이드 프로그램의 암호화 알고리즘이 달라서 불가능) 암호화X
            }

            //자동로그인확인값
            val autoLogin = cbAutoLogin.isChecked.let {
                if (it)
                    1
                else
                    0
            }

            //입력된 로그인데이터로 DB에서 데이터를 검색
            val getLoginData = memberManaguer.selectMemberData(loginData)

            if (getLoginData.password.equals(loginData.password)) {
                MemberData.setMemberData(getLoginData)

                //자동로그인 로그인사용자의 AutoLogin, ID, PW를 저장
                getSharedPreferences("auto", Activity.MODE_PRIVATE).edit().also {
                    it.putString("inputCheck",autoLogin.toString()).apply()
                    it.putString("inputPhone",loginData.phone).apply()
                    it.putString("inputPassword",loginData.password).apply()
                    it.commit()

                }

                //페이지 이동
                startActivity(mainIntent)

            }
        }
    }
}
