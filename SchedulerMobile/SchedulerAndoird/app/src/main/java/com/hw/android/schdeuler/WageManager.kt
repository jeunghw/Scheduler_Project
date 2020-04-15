package com.hw.android.schdeuler

import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.async
import kotlinx.coroutines.runBlocking

class ScheduleManager {

    fun selectScheduleData(phone : String, date : String) : MutableList<ScheduleData> {
        val scope = CoroutineScope(Dispatchers.Default)
        var result = arrayListOf<ScheduleData>()

        //코루틴을 사용해서 PHPManager을 실행, 결과값을 받음
        var jsonArray = scope.async {
            PHPManager().SelectScheduleData("SelectSchedule", "Phone="+phone+"&Date="+date)
        }

        try {

            runBlocking {
                //코루틴이 완료되길 기다림
                jsonArray.await()

                //완료된 코루틴내부의 json형식의 데이터를 logindata에 넣은뒤 반환
                var scheduleData = jsonArray.getCompleted().let {
                    var scheduleList = arrayListOf<ScheduleData>()
                    var sum = 0
                    for(i in 0..it.length()-1) {
                        var scheduleData = ScheduleData()
                        scheduleData.date = it.getJSONObject(i).getString("Date")
                        scheduleData.onTime = it.getJSONObject(i).getString("OnTime")
                        scheduleData.offTime = it.getJSONObject(i).getString("OffTime")
                        scheduleData.time = it.getJSONObject(i).getString("Time")
                        scheduleData.restTime = it.getJSONObject(i).getString("RestTime")
                        scheduleData.extensionTime = it.getJSONObject(i).getString("ExtensionTime")
                        scheduleData.nightTime = it.getJSONObject(i).getString("NightTime")
                        scheduleData.totalTime = it.getJSONObject(i).getString("TotalTime")
                        scheduleData.wage = it.getJSONObject(i).getString("Wage")
                        scheduleData.restWage = it.getJSONObject(i).getString("RestWage")
                        scheduleData.extensionWage = it.getJSONObject(i).getString("ExtensionWage")
                        scheduleData.nightWage = it.getJSONObject(i).getString("NightWage")
                        scheduleData.totalWage = it.getJSONObject(i).getString("TotalWage")

                        sum += scheduleData.totalWage.toInt()

                        scheduleList.add(scheduleData)
                    }

                    var scheduleData = ScheduleData()
                    scheduleData.nightWage = "합계"
                    scheduleData.totalWage = sum.toString()

                    scheduleList.add(scheduleData)

                    scheduleList
                }
                result = scheduleData
            }
        }
        catch (e : Exception)
        {
            System.out.println("MemberManager::selectScheduleData::Exception : "+e)
        }
        return result
    }
}