package com.hw.android.schdeuler

import kotlinx.coroutines.*

class MemberManager {

    //전달받은 데이터로 DB에 값이 있으면 해당 값을 반환
    fun selectMemberData(inputLoginData: LoginData) : LoginData {

        val scope = CoroutineScope(Dispatchers.Default)
        var result = LoginData()

        //코루틴을 사용해서 PHPManager을 실행, 결과값을 받음
        var jsonArray = scope.async {
            PHPManager().SelectMemberData("SelectMember", "Phone="+inputLoginData.phone)
        }

        try {

            runBlocking {
                //코루틴이 완료되길 기다림
                jsonArray.await()

                //완료된 코루틴내부의 json형식의 데이터를 logindata에 넣은뒤 반환
                var loginData = LoginData().let {
                    it.phone = jsonArray.getCompleted().getJSONObject(0).getString("Phone")
                    it.password = jsonArray.getCompleted().getJSONObject(0).getString("Password")
                    it.name = jsonArray.getCompleted().getJSONObject(0).getString("Name")
                    it.wage = jsonArray.getCompleted().getJSONObject(0).getString("Wage")
                    it.authority = jsonArray.getCompleted().getJSONObject(0).getString("Authority").toInt()

                    it
                }
                result = loginData
            }
        }
        catch (e : Exception)
        {
            System.out.println("MemberManager::selectLoginData::Exception : "+e)
        }
        return result
    }

    fun selectNameData() : MutableList<LoginData> {
        val scope = CoroutineScope(Dispatchers.Default)
        var result = mutableListOf<LoginData>()

        //코루틴을 사용해서 PHPManager을 실행, 결과값을 받음
        var jsonArray = scope.async {
            PHPManager().SelectMemberData("SelectName", "")
        }

        try {

            runBlocking {
                //코루틴이 완료되길 기다림
                jsonArray.await()

                //완료된 코루틴내부의 json형식의 데이터를 logindata에 넣은뒤 반환
                var loginData = jsonArray.getCompleted().let {
                    var namesList = mutableListOf<LoginData>()
                    for(i in 0..it.length()-1) {
                        var loginData = LoginData()
                        loginData.phone = it.getJSONObject(i).getString("Phone")
                        loginData.name = it.getJSONObject(i).getString("Name")
                        loginData.task = it.getJSONObject(i).getInt("task")
                        namesList.add(loginData)
                    }

                    namesList
                }
                result = loginData
            }
        }
        catch (e : Exception)
        {
            System.out.println("MemberManager::selectNameData::Exception : "+e)
        }
        return result
    }
}