package com.antonyjekov.nevix.player.buttons;

import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/25/2014.
 */
public class VolumeUpButton extends SystemVolumeUpButton {
    public VolumeUpButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    public String command() {
        return PlayerCommand.VOLUME_UP_CMD;
    }
}
