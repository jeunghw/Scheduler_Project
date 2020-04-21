package com.hw.android.schdeuler

import android.util.Log
import android.widget.Toast
import androidx.annotation.MainThread
import org.json.JSONArray
import org.json.JSONObject
import java.net.URL

import java.io.InputStreamReader

import java.io.OutputStreamWriter
import java.net.HttpURLConnection

class PHPManager {

    val serverIP = "http://15.164.228.119/"

    fun SelectMemberData(scriptName : String, writeData : String) : JSONArray {

        val url = URL(serverIP+scriptName+".php")
        var result = JSONArray()

        try {
            val conn = (url.openConnection() as HttpURLConnection).also {
                it.setRequestProperty("Content-Type", "application/x-www-form-urlencoded")
                it.requestMethod="POST"
                it.connect()
            }

            OutputStreamWriter(conn.getOutputStream()).also {
                it.write(writeData);
                it.flush()
                it.close()
            }

            var jsonArray = InputStreamReader(conn.getInputStream()).let {
                JSONObject(it.readText()).getJSONArray("Member")
            }

            result = jsonArray
        }
        catch (e : Exception) {
            System.out.println("예외  : " + e)
            System.out.println(writeData)
        }
        return result
    }

    fun SelectWageData(scriptName : String, writeData : String) : JSONArray {

        val url = URL(serverIP+scriptName+".php")
        var result = JSONArray()

        try {
            val conn = (url.openConnection() as HttpURLConnection).also {
                it.setRequestProperty("Content-Type", "application/x-www-form-urlencoded")
                it.requestMethod="POST"
                it.connect()
            }

            OutputStreamWriter(conn.getOutputStream()).also {
                it.write(writeData);
                it.flush()
                it.close()
            }

            var jsonArray = InputStreamReader(conn.getInputStream()).let {

                JSONObject(it.readText()).getJSONArray("Wage")
            }

            result = jsonArray
        }
        catch (e : Exception) {
            System.out.println("예외  : " + e)
            System.out.println(writeData)
        }
        return result
    }

    fun SelectScheduleData(scriptName : String, writeData : String) : JSONArray {

        val url = URL(serverIP+scriptName+".php")
        var result = JSONArray()

        try {
            val conn = (url.openConnection() as HttpURLConnection).also {
                it.setRequestProperty("Content-Type", "application/x-www-form-urlencoded")
                it.requestMethod="POST"
                it.connect()
            }

            OutputStreamWriter(conn.getOutputStream()).also {
                it.write(writeData);
                it.flush()
                it.close()
            }

            var jsonArray = InputStreamReader(conn.getInputStream()).let {
                JSONObject(it.readText()).getJSONArray("Schedule")
            }

            result = jsonArray
        }
        catch (e : Exception) {
            System.out.println("예외  : " + e)
            System.out.println(writeData)
        }
        return result
    }
}