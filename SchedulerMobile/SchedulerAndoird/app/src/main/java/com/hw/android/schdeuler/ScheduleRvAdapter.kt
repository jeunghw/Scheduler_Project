package com.hw.android.schdeuler

import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import kotlinx.android.synthetic.main.item_schedule_rceyclerview.view.*

class ScheduleRvAdapter(private val scheduleShowDataList: ArrayList<ScheduleShowData>) : RecyclerView.Adapter<ScheduleRvAdapter.ScheduleViewHolder>() {

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int) = ScheduleViewHolder(parent)

    override fun getItemCount() = scheduleShowDataList.size

    override fun onBindViewHolder(holder: ScheduleViewHolder, position: Int) {
        scheduleShowDataList[position].let { item ->
            with (holder) {
                name.text = item.name
                phone.text = item.phone
                monOnTime.text = item.monOnTime
                monOffTime.text = item.monOffTime
                tueOnTime.text = item.tueOnTime
                tueOffTime.text = item.tueOffTime
                wedOnTime.text = item.wedOnTime
                wedOffTime.text = item.wedOffTime
                thuOnTime.text = item.thuOnTime
                thuOffTime.text = item.thuOffTime
                friOnTime.text = item.friOnTime
                friOffTime.text = item.friOffTime
                satOnTime.text = item.satOnTime
                satOffTime.text = item.satOffTime
                sunOnTime.text = item.sunOnTime
                sunOffTime.text = item.sunOffTime
            }
        }
    }

    inner class ScheduleViewHolder(parent : ViewGroup) : RecyclerView.ViewHolder(
        LayoutInflater.from(parent.context).inflate(R.layout.item_schedule_rceyclerview, parent, false)) {
        val name = itemView.tvName
        val phone = itemView.tvPhone
        val monOnTime = itemView.tvMonOnTime
        val monOffTime = itemView.tvMonOffTime
        val tueOnTime = itemView.tvTueOnTime
        val tueOffTime = itemView.tvTueOffTime
        val wedOnTime = itemView.tvWedOnTime
        val wedOffTime = itemView.tvWedOffTime
        val thuOnTime = itemView.tvThuOnTime
        val thuOffTime = itemView.tvThuOffTime
        val friOnTime = itemView.tvFriOnTime
        val friOffTime = itemView.tvFriOffTime
        val satOnTime = itemView.tvSatOnTime
        val satOffTime = itemView.tvSatOffTime
        val sunOnTime = itemView.tvSunOnTime
        val sunOffTime = itemView.tvSunOffTime
    }
}