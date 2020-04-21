package com.hw.android.schdeuler

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.Fragment
import kotlinx.android.synthetic.main.layout_main.*


class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.layout_main)

        //mFragmentManager.beginTransaction().add(R.id.fragment, MainFragment()).commit()
        val fragmentTransient = supportFragmentManager.beginTransaction()
        fragmentTransient.add(R.id.fragment, MainFragment()).commit()

        btnLogout.setOnClickListener {
            finish()
        }
    }

    fun replaceFragment(fragment: Fragment) {
        val fragmentTransient = supportFragmentManager.beginTransaction()

        fragmentTransient.replace(R.id.fragment, fragment).commit()
    }

    //뒤로가기 버튼을 누르면 앱종료를 위해 재정의
    override fun onBackPressed() {
       // super.onBackPressed()
        finishAffinity()
    }

}