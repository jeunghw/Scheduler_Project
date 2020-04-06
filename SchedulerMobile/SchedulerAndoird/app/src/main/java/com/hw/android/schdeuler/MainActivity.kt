package com.hw.android.schdeuler

import android.os.Bundle
import android.widget.ArrayAdapter
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.activity_main.*
import java.lang.Exception
import java.util.*
import kotlin.collections.ArrayList

class MainActivity : AppCompatActivity() {

    object DataList {
        var loginData = ArrayList<LoginData>()
    }
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        inItSpinner()

        var scheduleList = arrayListOf<ScheduleData>()

        btnSearch.setOnClickListener {
            var scheduleManager = ScheduleManager()
            var nameIndex = spName.selectedItemPosition
            var year = spYear.selectedItem.toString().substring(0,spYear.selectedItem.toString().length-1)
            var month = spMonth.selectedItem.toString().substring(0,spMonth.selectedItem.toString().length-1)

            //선택된 이름의 인덱스와 년도, 월을 넘겨주고 DB에서 검색해온 리스트를 받음
            var scheduleList = scheduleManager.selectScheduleData(DataList.loginData.get(nameIndex).phone, year+"-0"+month+"-")
            
            //받아온 값을 로그로 출력
            System.out.println(scheduleList)

            //레거시뷰를 생성해서 데이터를 연결
            try {
                findViewById<RecyclerView>(R.id.rlschedule).apply {
                    setHasFixedSize(true)

                    layoutManager = LinearLayoutManager(this@MainActivity)
                    adapter = ScheduleRvAdapter(scheduleList as ArrayList<ScheduleData>)
                }
            }
            catch (e : Exception)
            {
                System.out.println("MainActivity:btnSearch.setOnClickListener : "+e.message)
            }


        }

        btnLogout.setOnClickListener {
            finish()
        }
    }

    fun inItSpinner() {
        var namesList = ArrayList<String>()

        //다른 사용자의 검색권한이 있으면
        if (MemberData.authorityData.authority==1 || MemberData.authorityData.authority==2) {
            //이름스패너 초기화
            val managuer = MemberManager()
            namesList = managuer.selectNameData().let {
                var loginDataList = ArrayList<LoginData>()
                var nameList = ArrayList<String>()
                for (i in 1..it.size - 1) {
                    loginDataList.add(it.get(i))
                    nameList.add(it.get(i).name)
                }

                DataList.loginData = loginDataList

                nameList
            }

        }
        //자기자신의 데이터만 검색이 가능하면
        else
        {
            namesList.add(MemberData.name)
        }

        spName.adapter = ArrayAdapter(this, R.layout.item_spinner, namesList)
        spName.setSelection(0)
        //년도스패너 초기화
        val yearItem = ArrayList<String>()
        //현재 년도 가져오기
        var nowYear = GregorianCalendar(TimeZone.getTimeZone("Asia/Seoul")).get(GregorianCalendar.YEAR)

        for (year in nowYear-5..nowYear+5)
        {
            yearItem.add(year.toString()+"년")
        }

        spYear.adapter = ArrayAdapter(this, R.layout.item_spinner, yearItem)
        spYear.setSelection(5)

        //월스패너 초기화
        val monthItem = ArrayList<String>()
        //현재 월 가져오기
        var nowMonth = GregorianCalendar(TimeZone.getTimeZone("Asia/Seoul")).get(GregorianCalendar.MONTH)

        for(month in 1..12)
        {
            monthItem.add(month.toString()+"월")
        }
        
        spMonth.adapter = ArrayAdapter(this, R.layout.item_spinner, monthItem)
        spMonth.setSelection(nowMonth)
    }


    //뒤로가기 버튼을 누르면 앱종료를 위해 재정의
    override fun onBackPressed() {
       // super.onBackPressed()
        finishAffinity()
    }

}