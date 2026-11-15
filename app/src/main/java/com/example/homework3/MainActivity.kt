
package com.example.homework3

import android.Manifest
import android.app.NotificationManager
import android.content.Intent
import android.content.pm.PackageManager
import android.net.Uri
import android.os.Build
import android.os.Bundle
import android.provider.Settings
import android.widget.Toast
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.rememberScrollState
import androidx.compose.foundation.verticalScroll
import androidx.compose.material3.Button
import androidx.compose.material3.Checkbox
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.core.content.ContextCompat
import com.example.homework3.ui.theme.HomeWork3Theme

class MainActivity : ComponentActivity() {

    private var hasNotificationPermission by mutableStateOf(false)

    private val permissionLauncher =
        registerForActivityResult(
            ActivityResultContracts.RequestPermission()
        ) { granted ->
            hasNotificationPermission = granted
        }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        setContent {
            HomeWork3Theme {
                Scaffold(
                    modifier = Modifier.fillMaxSize()
                ) { innerPadding ->

                    if (hasNotificationPermission) {
                        MainScreen(
                            modifier = Modifier.padding(innerPadding)
                        )
                    } else {
                        PermissionScreen(
                            modifier = Modifier.padding(innerPadding)
                        )
                    }
                }
            }
        }

        checkNotificationPermission()

        if (intent.getBooleanExtra("from_notification", false)) {
            val title = intent.getStringExtra("notification_title") ?: ""

            Toast.makeText(
                this,
                title,
                Toast.LENGTH_LONG
            ).show()
        }
    }

    private fun checkNotificationPermission() {

        if (Build.VERSION.SDK_INT >= 33) {

            if (ContextCompat.checkSelfPermission(
                    this,
                    Manifest.permission.POST_NOTIFICATIONS
                ) == PackageManager.PERMISSION_GRANTED
            ) {
                hasNotificationPermission = true
            } else {
                window.decorView.post {
                    permissionLauncher.launch(
                        Manifest.permission.POST_NOTIFICATIONS
                    )
                }
            }

        } else {
            hasNotificationPermission = true
        }
    }

    override fun onResume() {
        super.onResume()

        if (Build.VERSION.SDK_INT >= 33) {
            hasNotificationPermission =
                ContextCompat.checkSelfPermission(
                    this,
                    Manifest.permission.POST_NOTIFICATIONS
                ) == PackageManager.PERMISSION_GRANTED
        } else {
            hasNotificationPermission = true
        }
    }

    override fun onNewIntent(intent: Intent) {
        super.onNewIntent(intent)
        setIntent(intent)

        if (intent.getBooleanExtra("from_notification", false)) {
            val message = intent.getStringExtra("notification_text") ?: ""

            Toast.makeText(
                this,
                message,
                Toast.LENGTH_LONG
            ).show()
        }
    }

    @Composable
    private fun PermissionScreen(modifier: Modifier = Modifier) {

        Column(
            modifier = modifier
                .fillMaxSize()
                .padding(20.dp),
            verticalArrangement = Arrangement.Center
        ) {

            Text(
                text = stringResource(R.string.permission_warning)
            )

            Button(
                onClick = {
                    val settingsIntent = Intent(
                        Settings.ACTION_APPLICATION_DETAILS_SETTINGS,
                        Uri.fromParts("package", packageName, null)
                    )

                    startActivity(settingsIntent)
                },
                modifier = Modifier.padding(top = 16.dp)
            ) {
                Text(
                    text = stringResource(R.string.open_settings)
                )
            }
        }
    }

    @Composable
    private fun MainScreen(modifier: Modifier = Modifier) {

        var title by remember { mutableStateOf("") }
        var message by remember { mutableStateOf("") }

        var nonCancellable by remember {
            mutableStateOf(false)
        }

        var selectedImportance by remember {
            mutableStateOf(NotificationManager.IMPORTANCE_DEFAULT)
        }

        var expanded by remember {
            mutableStateOf(false)
        }

        val importanceText = when (selectedImportance) {

            NotificationManager.IMPORTANCE_MIN ->
                stringResource(R.string.importance_min)

            NotificationManager.IMPORTANCE_HIGH ->
                stringResource(R.string.importance_high)

            else ->
                stringResource(R.string.importance_default)
        }

        Column(
            modifier = modifier
                .fillMaxSize()
                .verticalScroll(rememberScrollState())
                .padding(16.dp),
            verticalArrangement = Arrangement.spacedBy(12.dp)
        ) {

            TextField(
                value = title,
                onValueChange = { title = it },
                label = {
                    Text(stringResource(R.string.title_hint))
                },
                modifier = Modifier.fillMaxWidth()
            )

            TextField(
                value = message,
                onValueChange = { message = it },
                label = {
                    Text(stringResource(R.string.message_hint))
                },
                modifier = Modifier.fillMaxWidth()
            )

            Row(
                modifier = Modifier.fillMaxWidth()
            ) {

                Checkbox(
                    checked = nonCancellable,
                    onCheckedChange = {
                        nonCancellable = it
                    }
                )

                Text(
                    text = stringResource(R.string.non_cancellable)
                )
            }

            Text(
                text = stringResource(R.string.importance_label)
            )

            Column {

                Button(
                    onClick = {
                        expanded = true
                    }
                ) {
                    Text(importanceText)
                }

                DropdownMenu(
                    expanded = expanded,
                    onDismissRequest = {
                        expanded = false
                    }
                ) {

                    DropdownMenuItem(
                        text = {
                            Text(stringResource(R.string.importance_min))
                        },
                        onClick = {
                            selectedImportance =
                                NotificationManager.IMPORTANCE_MIN
                            expanded = false
                        }
                    )

                    DropdownMenuItem(
                        text = {
                            Text(stringResource(R.string.importance_default))
                        },
                        onClick = {
                            selectedImportance =
                                NotificationManager.IMPORTANCE_DEFAULT
                            expanded = false
                        }
                    )

                    DropdownMenuItem(
                        text = {
                            Text(stringResource(R.string.importance_high))
                        },
                        onClick = {
                            selectedImportance =
                                NotificationManager.IMPORTANCE_HIGH
                            expanded = false
                        }
                    )
                }
            }

            Button(
                onClick = {

                    val notificationTitle = title.ifBlank {
                        getString(R.string.default_notification_title)
                    }

                    val notificationMessage = message.ifBlank {
                        getString(R.string.default_notification_message)
                    }

                    NotificationHelper.showNotification(
                        context = this@MainActivity,
                        title = notificationTitle,
                        message = notificationMessage,
                        nonCancellable = nonCancellable,
                        importance = selectedImportance
                    )
                },
                modifier = Modifier.fillMaxWidth()
            ) {
                Text(
                    stringResource(R.string.send_notification)
                )
            }
        }
    }
}
