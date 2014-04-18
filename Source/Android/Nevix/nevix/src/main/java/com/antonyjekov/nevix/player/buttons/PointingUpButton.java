package com.antonyjekov.nevix.player.buttons;

import android.graphics.Path;
import android.graphics.Rect;

import com.antonyjekov.nevix.constants.PlayerCommand;

/**
 * Created by Antony Jekov on 3/27/2014.
 */
public class PointingUpButton extends SphericalButton {
    public PointingUpButton(Rect button, int strokeWidth, int padding) {
        super(button, strokeWidth, padding);
    }

    @Override
    protected void prepareShape(Path shape) {
        super.prepareShape(shape);

        int x = button.centerX();
        int y = button.top + padding;

        int halfLen = (button.width() - (padding << 1)) >> 1;

        shape.moveTo(x, y);
        shape.lineTo(x - halfLen, button.centerY());
        shape.lineTo(x + halfLen, button.centerY());
        shape.lineTo(x, y);
        shape.close();
    }

    @Override
    public String command() {
        return PlayerCommand.BROWSE_CMD;
    }
}
