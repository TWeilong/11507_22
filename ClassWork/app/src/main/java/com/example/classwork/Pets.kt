package com.example.classwork

import java.io.Serializable

data class Pets(
    val nameId: Int,
    val infoId: Int,
    val ageId: Int,
    val imageId: Int
) : Serializable