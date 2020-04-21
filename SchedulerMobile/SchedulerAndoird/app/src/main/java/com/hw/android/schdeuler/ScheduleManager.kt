package com.hw.android.schdeuler

import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.async
import kotlinx.coroutines.runBlocking
import java.lang.Exception
import java.text.SimpleDateFormat
import java.util.*
import java.util.Collections.addAll
import java.util.Collections.copy
import kotlin.collections.ArrayList
import kotlin.collections.HashMap

class ScheduleManager {

    //핸드폰, 시작날짜, 마지막날짜를 입력받아 근태관리 데이터를 검색해 리스트로 반환
    fun selectScheduleData(phone : String, startDate : String, endDate : String) :  ArrayList<ScheduleData> {
        val scope = CoroutineScope(Dispatchers.Default)
        var result = ArrayList<ScheduleData>()
        var writeData = ""

        var jsonArray = scope.async {
            if(!phone.equals(""))
            {
                writeData = "phone="+phone+"&"
            }

            writeData = writeData +"startDate="+startDate+"&endDate="+endDate

            PHPManager().SelectScheduleData("SelectSchedule",writeData)
        }

        try {
            runBlocking {
                jsonArray.await()


                var scheduleShowList = jsonArray.getCompleted().let {

                    var scheduleDataList = ArrayList<ScheduleData>()
                    var phone = ""
                    for(i in 0..it.length()-1) {
                        var scheduleData = ScheduleData()
                        scheduleData.date = it.getJSONObject(i).getString("Date")
                        scheduleData.phone = it.getJSONObject(i).getString("Phone")
                        scheduleData.onTime = it.getJSONObject(i).getString("OnTime")
                        scheduleData.offTime = it.getJSONObject(i).getString("OffTime")

                        scheduleDataList.add(scheduleData);
                    }

                    scheduleDataList
                }
                result = scheduleShowList
            }
        }
        catch (e : Exception)
        {
            System.out.println("SchdeuleManager::selectScheduleData::Exception : "+e)
        }

        return result;
    }

    //View에 보여주기 위해서 해당 데이터 형식으로 변환
    fun getScheduleShowDataList(scheduleDataList : ArrayList<ScheduleData>, loginDataList : ArrayList<LoginData>, dateList : ArrayList<String>) : ArrayList<ScheduleShowData>
    {
        var scheduleShowDataList = ArrayList<ScheduleShowData>()
        var scheduleShowData = ScheduleShowData()

        var index = 0
        var phone = ""
        while(true) {
            if(!phone.equals(scheduleDataList[index].phone)) {
                if(!phone.equals("")) {
                    scheduleShowDataList.add(scheduleShowData.copy())
                    scheduleShowData = ScheduleShowData()
                }

                
                phone = scheduleDataList[index].phone
                scheduleShowData.phone = phone
                for(i in 0..loginDataList.size-1) {
                    if (phone.equals(loginDataList[i].phone)) {
                        scheduleShowData.name = loginDataList[i].name
                        break
                    }
                }
            }
            for(i in 0..6) {
                if(scheduleDataList[index].date == dateList[i]) {
                    if(i==0) {
                        scheduleShowData.monOnTime = scheduleDataList[index].onTime
                        scheduleShowData.monOffTime = scheduleDataList[index].offTime
                    } else if(i==1) {
                        scheduleShowData.tueOnTime = scheduleDataList[index].onTime
                        scheduleShowData.tueOffTime = scheduleDataList[index].offTime
                    } else if(i==2) {
                        scheduleShowData.wedOnTime = scheduleDataList[index].onTime
                        scheduleShowData.wedOffTime = scheduleDataList[index].offTime
                    } else if(i==3) {
                        scheduleShowData.thuOnTime = scheduleDataList[index].onTime
                        scheduleShowData.thuOffTime = scheduleDataList[index].offTime
                    } else if(i==4) {
                        scheduleShowData.friOnTime = scheduleDataList[index].onTime
                        scheduleShowData.friOffTime = scheduleDataList[index].offTime
                    }else if(i==5) {
                        scheduleShowData.satOnTime = scheduleDataList[index].onTime
                        scheduleShowData.satOffTime = scheduleDataList[index].offTime
                    } else if(i==6) {
                        scheduleShowData.sunOnTime = scheduleDataList[index].onTime
                        scheduleShowData.sunOffTime = scheduleDataList[index].offTime
                    }
                }
            }
            index++

            if(index == scheduleDataList.size)
            {
                scheduleShowDataList.add(scheduleShowData.copy())
                break
            }
        }

        return scheduleShowDataList
    }

    //특정 년, 월, 주를 받아서 해당 주의 날짜와 요일을 매칭한뒤 리스트로 반환
    fun getDateList(year : Int, month : Int, week : Int) : ArrayList<String>
    {
        val dateList = ArrayList<String>()
        var date = GregorianCalendar(TimeZone.getTimeZone("Asia/Seoul")).let {
            it.set(Calendar.YEAR, year)
            it.set(Calendar.MONTH, month-1)
            it.set(Calendar.WEEK_OF_MONTH, week)
            it.set(Calendar.DAY_OF_WEEK,Calendar.MONDAY)
            SimpleDateFormat("yyyy-MM-dd").format(it.time)
        }
        val swap = date.split('-')

        val nYear = swap[0].toInt()
        var nMonth = swap[1].toInt()
        var nDay = swap[2].toInt()

        val monthLastDay = GregorianCalendar(TimeZone.getTimeZone("Asia/Seoul")).let {
            it.set(Calendar.YEAR, year)
            it.set(Calendar.MONTH, month - 1)
            it.getActualMaximum(Calendar.DAY_OF_MONTH)
        }
        var nMonthLastday = -1;

        if(nMonth < month)
        {
            nMonthLastday = GregorianCalendar(TimeZone.getTimeZone("Asia/Seoul")).let {
                it.set(Calendar.YEAR, year)
                it.set(Calendar.MONTH, nMonth-1)
                it.getActualMaximum(Calendar.DAY_OF_MONTH)
            }
        }
        for(i in 0..6)
        {
            dateList.add(nYear.toString()+"-"+String.format("%02d",nMonth)+"-"+String.format("%02d",nDay++))
            if(nMonthLastday != -1 && nDay > nMonthLastday)
            {
                nDay = nDay - nMonthLastday
                nMonth++
            }
            if(nMonth== month && nDay > monthLastDay) {
                nDay = nDay - monthLastDay
                nMonth++
            }
        }

        return dateList;
    }


}