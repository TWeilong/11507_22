
package com.example.homework3

import android.app.NotificationChannel
import android.app.NotificationManager
import android.app.PendingIntent
import android.content.Context
import android.content.Intent
import android.os.Build
import androidx.core.app.NotificationCompat
import androidx.core.app.NotificationManagerCompat

object NotificationHelper {

    fun showNotification(
        context: Context,
        title: String,
        message: String,
        nonCancellable: Boolean,
        importance: Int
    ) {

        val channelId = "notification_channel_$importance"

        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {

            val channelName = context.getString(
                R.string.notification_channel_name
            )

            val channel = NotificationChannel(
                channelId,
                channelName,
                importance
            )

            val manager = context.getSystemService(
                Context.NOTIFICATION_SERVICE
            ) as NotificationManager

            manager.createNotificationChannel(channel)
        }

        val notificationId = System.currentTimeMillis().toInt()

        val intent = Intent(
            context,
            MainActivity::class.java
        ).apply {

            flags = Intent.FLAG_ACTIVITY_CLEAR_TOP or
                    Intent.FLAG_ACTIVITY_SINGLE_TOP

            putExtra("from_notification", true)
            putExtra("notification_title", title)
            putExtra("notification_text", message)
        }

        val pendingIntent = PendingIntent.getActivity(
            context,
            notificationId,
            intent,
            PendingIntent.FLAG_UPDATE_CURRENT or
                    PendingIntent.FLAG_IMMUTABLE
        )

        val notification = NotificationCompat.Builder(
            context,
            channelId
        )
            .setSmallIcon(android.R.drawable.ic_dialog_info)
            .setContentTitle(title)
            .setContentText(message)
            .setStyle(
                NotificationCompat.BigTextStyle()
                    .bigText(message)
            )
            .setPriority(
                when (importance) {

                    NotificationManager.IMPORTANCE_MIN ->
                        NotificationCompat.PRIORITY_MIN

                    NotificationManager.IMPORTANCE_HIGH ->
                        NotificationCompat.PRIORITY_HIGH

                    else ->
                        NotificationCompat.PRIORITY_DEFAULT
                }
            )
            .setContentIntent(pendingIntent)
            .setAutoCancel(!nonCancellable)
            .setOngoing(nonCancellable)
            .build()

        NotificationManagerCompat
            .from(context)
            .notify(notificationId, notification)
    }
}
