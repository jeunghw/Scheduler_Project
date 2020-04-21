package com.hw.android.schdeuler

object MemberData {

    var phone = ""
    var password = ""
    var name = ""
    var wage=""
    var authorityData = AuthorityData()
    var task = -1;

    fun setMemberData(loginData: LoginData){
        phone = loginData.phone
        password = loginData.password
        name = loginData.name
        wage = loginData.wage
        authorityData.authority = loginData.authority
        task = loginData.task;
    }
}