package com.example.homework1

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Column
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.res.stringResource
import com.example.homework1.ui.theme.HomeWork1Theme

class MainActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            HomeWork1Theme {
                FirstScreen()
            }
        }
    }

    @Composable
    fun FirstScreen() {

        val name = stringResource(R.string.student_name)
        val group = stringResource(R.string.group_number)

        Column {
            Text(text = name)
            Text(text = group)

            Button(
                onClick = {
                    val intent = Intent(this@MainActivity, SecondActivity::class.java)

                    intent.putExtra("student_info", "$name/$group")

                    startActivity(intent)
                }
            ) {
                Text(text = stringResource(R.string.button))
            }
        }
    }
}