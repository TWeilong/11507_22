package com.example.classwork

import android.content.Intent
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.example.classwork.ui.theme.ClassWorkTheme

class MainActivity : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            ClassWorkTheme {
                FirstScreen()
            }
        }
    }

    @Composable
    fun FirstScreen() {

        val cat = Pets(
            nameId = R.string.cat,
            infoId = R.string.cat_info,
            ageId = R.string.cat_age,
            imageId = R.drawable.cat
        )

        val dog = Pets(
            nameId = R.string.dog,
            infoId = R.string.dog_info,
            ageId = R.string.dog_age,
            imageId = R.drawable.dog
        )

        Column(
            modifier = Modifier.padding(top = 40.dp)
        ) {

            Button(
                onClick = {
                    val intent = Intent(
                        this@MainActivity,
                        SecondAct::class.java
                    )

                    intent.putExtra("pet", cat)

                    startActivity(intent)
                }
            ) {
                Text(
                    text = stringResource(R.string.cat_button)
                )
            }

            Button(
                onClick = {
                    val intent = Intent(
                        this@MainActivity,
                        SecondAct::class.java
                    )

                    intent.putExtra("pet", dog)

                    startActivity(intent)
                }
            ) {
                Text(
                    text = stringResource(R.string.dog_button)
                )
            }
        }
    }
}