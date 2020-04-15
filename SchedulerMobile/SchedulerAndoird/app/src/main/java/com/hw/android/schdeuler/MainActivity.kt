package com.hw.android.schdeuler

import android.os.Bundle
import android.widget.ArrayAdapter
import androidx.appcompat.app.AppCompatActivity
import androidx.fragment.app.FragmentManager
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.activity_main.*
import kotlinx.android.synthetic.main.activity_main.view.*
import kotlinx.android.synthetic.main.fragment_layout.*
import java.lang.Exception
import java.util.*
import kotlin.collections.ArrayList

class MainActivity : AppCompatActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        supportFragmentManager.beginTransaction().replace(R.id.fragment, MainFragment()).commit()

        btnWage.setOnClickListener {
            fragment.


        }

        btnLogout.setOnClickListener {
            finish()
        }
    }

    //뒤로가기 버튼을 누르면 앱종료를 위해 재정의
    override fun onBackPressed() {
       // super.onBackPressed()
        finishAffinity()
    }

}