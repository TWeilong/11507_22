package com.example.homework1

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.material3.Text
import com.example.homework1.ui.theme.HomeWork1Theme

class SecondActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val studentInfo = intent.getStringExtra("student_info") ?: ""

        setContent {
            HomeWork1Theme {
                Text(text = studentInfo)
            }
        }
    }
}