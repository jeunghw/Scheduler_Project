package com.hw.android.schdeuler

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.item_wage_rceyclerview.view.*

//리스트를 받아서 레거시뷰에 연결
class WageRvAdapter(private val wageList : ArrayList<WageData>) : RecyclerView.Adapter<WageRvAdapter.WageViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = WageViewHolder(parent)

    //아이템 길이
    override fun getItemCount() = wageList.size

    override fun onBindViewHolder(holder: WageViewHolder, position: Int) {
        wageList[position].let { item ->
            with (holder) {
                var swap = item.date.split("-")
                if(swap.size < 2) {
                    Day.text = "";
                }
                else {
                    Day.text = swap[2]
                }
                OnTime.text = item.onTime
                OffTime.text = item.offTime
                Time.text = item.time
                RestTime.text = item.restTime
                ExtensionTime.text = item.extensionTime
                NightTime.text = item.nightTime
                TotalTime.text = item.totalTime
                Wage.text = item.wage
                RestWage.text = item.restWage
                ExtensionWage.text = item.extensionWage
                NightWage.text = item.nightWage
                TotalWage.text = item.totalWage
            }
        }
    }
    
    //변수를 생성해서 아이템 뷰의 아이템과 매칭
    inner class WageViewHolder(parent: ViewGroup) : RecyclerView.ViewHolder(
        LayoutInflater.from(parent.context).inflate(R.layout.item_wage_rceyclerview, parent, false)) {
        val Day = itemView.tbDay
        val OnTime = itemView.tbOnTime
        val OffTime = itemView.tbOffTime
        val Time = itemView.tbTime
        val RestTime = itemView.tbRestTime
        val ExtensionTime = itemView.tbExtensionTime
        val NightTime = itemView.tbNightTime
        val TotalTime = itemView.tbTotalTime
        val Wage = itemView.tbWage
        val RestWage = itemView.tbRestWage
        val ExtensionWage = itemView.tbExtensionWage
        val NightWage = itemView.tbNightWage
        val TotalWage = itemView.tbTotalWage
    }

}