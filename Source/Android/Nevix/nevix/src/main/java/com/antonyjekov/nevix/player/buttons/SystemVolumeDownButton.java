package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/25/2014.
 */
public class SystemVolumeDownButton extends SphericalButton {
    public SystemVolumeDownButton(Rect button) {
        super(button);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        shape.moveTo(button.left + padding, button.centerY());
        shape.lineTo(button.right - padding, button.centerY());
    }

    @Override
    public String command() {
        return PlayerCommand.SYS_VOLUME_DOWN_CMD;
    }
}
