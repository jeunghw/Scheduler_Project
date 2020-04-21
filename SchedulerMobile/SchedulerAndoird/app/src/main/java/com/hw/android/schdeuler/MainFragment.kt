package com.hw.android.schdeuler

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import androidx.fragment.app.Fragment
import kotlinx.android.synthetic.main.fragment_main.*

class MainFragment : Fragment() {

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View {
        val view = inflater.inflate(R.layout.fragment_main, container, false)

        view.findViewById<Button>(R.id.btnWage).setOnClickListener {
            val intent = Intent(view.context,WageActivity::class.java)
            startActivity(intent)
        }

        view.findViewById<Button>(R.id.btnSchedule).setOnClickListener {
            val intent = Intent(view.context, ScheduleActivity::class.java)
            startActivity(intent)
        }

        return view

    }
}