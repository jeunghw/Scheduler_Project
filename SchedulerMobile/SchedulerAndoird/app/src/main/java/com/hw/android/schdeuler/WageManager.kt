package com.hw.android.schdeuler

import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.async
import kotlinx.coroutines.runBlocking

class WageManager {

    fun selectWageData(phone : String, date : String) : MutableList<WageData> {
        val scope = CoroutineScope(Dispatchers.Default)
        var result = arrayListOf<WageData>()

        //코루틴을 사용해서 PHPManager을 실행, 결과값을 받음
        var jsonArray = scope.async {
            PHPManager().SelectWageData("SelectWage", "Phone="+phone+"&Date="+date)
        }

        try {

            runBlocking {
                //코루틴이 완료되길 기다림
                jsonArray.await()

                //완료된 코루틴내부의 json형식의 데이터를 logindata에 넣은뒤 반환
                var wageData = jsonArray.getCompleted().let {
                    var wageList = arrayListOf<WageData>()
                    var sum = 0
                    for(i in 0..it.length()-1) {
                        var wageData = WageData()
                        wageData.date = it.getJSONObject(i).getString("Date")
                        wageData.onTime = it.getJSONObject(i).getString("OnTime")
                        wageData.offTime = it.getJSONObject(i).getString("OffTime")
                        wageData.time = it.getJSONObject(i).getString("Time")
                        wageData.restTime = it.getJSONObject(i).getString("RestTime")
                        wageData.extensionTime = it.getJSONObject(i).getString("ExtensionTime")
                        wageData.nightTime = it.getJSONObject(i).getString("NightTime")
                        wageData.totalTime = it.getJSONObject(i).getString("TotalTime")
                        wageData.wage = it.getJSONObject(i).getString("Wage")
                        wageData.restWage = it.getJSONObject(i).getString("RestWage")
                        wageData.extensionWage = it.getJSONObject(i).getString("ExtensionWage")
                        wageData.nightWage = it.getJSONObject(i).getString("NightWage")
                        wageData.totalWage = it.getJSONObject(i).getString("TotalWage")

                        sum += wageData.totalWage.toInt()

                        wageList.add(wageData)
                    }

                    var wageData = WageData()
                    wageData.nightWage = "합계"
                    wageData.totalWage = sum.toString()

                    wageList.add(wageData)

                    wageList
                }
                result = wageData
            }
        }
        catch (e : Exception)
        {
            System.out.println("MemberManager::selectScheduleData::Exception : "+e)
        }
        return result
    }
}