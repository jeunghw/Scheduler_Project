package com.hw.android.schdeuler

import android.os.Bundle
import android.view.View
import android.widget.AdapterView
import android.widget.ArrayAdapter
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.size
import androidx.recyclerview.widget.LinearLayoutManager
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.layout_schedule.*
import java.lang.Exception
import java.util.*
import kotlin.collections.ArrayList

class ScheduleActivity : AppCompatActivity(), AdapterView.OnItemSelectedListener {

    var loginDataList = ArrayList<LoginData>()
    var dateList = ArrayList<String>()
    var scheduleManager = ScheduleManager()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.layout_schedule)

        inItSpinner()
        inItLayout()

        btnSearch.setOnClickListener {
            inItLayout()

            var phone ="";
            val startDate = dateList[0]
            val endDate = dateList[dateList.size-1]

            if(spName.selectedItemPosition != 0)
                phone = spName.selectedItem.toString()

            var scheduleDataList = scheduleManager.selectScheduleData(phone, startDate, endDate)
            var scheduleShowDataList = ArrayList<ScheduleShowData>()

            if(!scheduleDataList.isEmpty()) {
                scheduleShowDataList = scheduleManager.getScheduleShowDataList(
                    scheduleDataList,
                    loginDataList,
                    dateList
                )
            }
            //레거시뷰를 생성해서 데이터를 연결
            try {
                rlsSchedule.apply {
                    setHasFixedSize(true)

                    layoutManager = LinearLayoutManager(this@ScheduleActivity)
                    adapter =
                        ScheduleRvAdapter(scheduleShowDataList as ArrayList<ScheduleShowData>)
                }
            } catch (e: Exception) {
                System.out.println("MainActivity:btnSearch.setOnClickListener : " + e.message)
            }

        }

        spTask.onItemSelectedListener = this
    }

    fun inItLayout()
    {
        var year = spYear.selectedItem.toString().substring(0,spYear.selectedItem.toString().length-1).toInt()
        var month = spMonth.selectedItem.toString().substring(0,spMonth.selectedItem.toString().length-1).toInt()
        var week = spWeek.selectedItem.toString().substring(0,spMonth.selectedItem.toString().length-1).toInt()
        dateList = ScheduleManager().getDateList(year, month, week)
        val tvList = arrayOf(tvMonDate, tvTueDate, tvWedDate, tvThuDate, tvFriDate, tvSatDate, tvSunDate)

        for(i in 0..6)
        {
            tvList[i].text = tvList[i].text.toString().substring(0,1) +"("+dateList[i].split('-')[1]+"/"+dateList[i].split('-')[2]+")";
        }
    }

    fun inItSpinner() {

        //분류스패너 초기화
        val taskItem = ArrayList<String>()
        taskItem.add("무관")
        taskItem.add("홀")
        taskItem.add("주방")

        spTask.adapter = ArrayAdapter(this, R.layout.item_spinner, taskItem)
        spTask.setSelection(MemberData.task)

        //inItNameSpinner()

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

        //주스패너 초기화
        val weekItem = ArrayList<String>()
        //현재 주 가져오기
        var nowWeek = GregorianCalendar(TimeZone.getTimeZone("Asia/Seoul")).get(GregorianCalendar.WEEK_OF_MONTH)

        for(week in 1..5)
        {
            weekItem.add(week.toString()+"주")
        }

        spWeek.adapter = ArrayAdapter(this, R.layout.item_spinner, weekItem)
        spWeek.setSelection(nowWeek-1)
    }

    //이름스패너 초기화
    fun inItNameSpinner()
    {
        var namesList = ArrayList<String>()

        //다른 사용자의 검색권한이 있으면
        if (MemberData.authorityData.authority==1 || MemberData.authorityData.authority==2) {
            //이름스패너 초기화
            namesList.add("전체")
            for (i in 0..loginDataList.size-1) {
                namesList.add(loginDataList[i].name)
            }

        }
        //자기자신의 데이터만 검색이 가능하면
        else
        {
            namesList.add("전체")
            namesList.add(MemberData.name)
        }
        spName.adapter = ArrayAdapter(this, R.layout.item_spinner, namesList)
        spName.setSelection(0)
    }

    override fun onNothingSelected(parent: AdapterView<*>?) {
        TODO("Not yet implemented")
    }

    override fun onItemSelected(parent: AdapterView<*>?, view: View?, position: Int, id: Long) {

        val managuer = MemberManager()
        loginDataList = managuer.selectNameData()

        var swapLoginData = ArrayList<LoginData>()
        for(i in 0..loginDataList.size-1)
        {
            if(loginDataList[i].phone.equals("00000000000"))
            {
                continue
            }
            if(position == 1)
            {
                if(loginDataList[i].task == 1 ||loginDataList[i].task==0)
                    swapLoginData.add(loginDataList[i])
            }
            else if(position == 2)
            {
                if(loginDataList[i].task == 2 || loginDataList[i].task==0)
                    swapLoginData.add(loginDataList[i])
            }
            else
            {
                swapLoginData.add(loginDataList[i])
            }
        }
        loginDataList = swapLoginData
        inItNameSpinner()
    }
}