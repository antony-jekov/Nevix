package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/25/2014.
 */
public class SystemVolumeUpButton extends SystemVolumeDownButton {
    public SystemVolumeUpButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        shape.moveTo(button.centerX(), button.top + padding);
        shape.lineTo(button.centerX(), button.bottom - padding);
    }

    @Override
    public String command() {
        return PlayerCommand.SYS_VOLUME_UP_CMD;
    }
}
