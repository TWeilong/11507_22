package com.example.classwork

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Column
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.res.stringResource
import com.example.classwork.ui.theme.ClassWorkTheme

class SecondAct : ComponentActivity() {

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        val pet = intent.getSerializableExtra("pet") as Pets

        setContent {
            ClassWorkTheme {
                SecondScreen(pet)
            }
        }
    }

    @Composable
    fun SecondScreen(pet: Pets) {

        Column {

            Image(
                painter = painterResource(pet.imageId),
                contentDescription = stringResource(pet.nameId)
            )

            Text(
                text = stringResource(pet.nameId)
            )

            Text(
                text = stringResource(pet.infoId)
            )

            Text(
                text = stringResource(pet.ageId)
            )
        }
    }
}