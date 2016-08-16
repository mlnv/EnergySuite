//
//  iOSBridge.m
//  UnityPligin
//
//  Created by Mlnv on 11.08.16.
//  Copyright © 2016 Mlnv. All rights reserved.
//

#import "ESiOSBridge.h"
#include <sys/sysctl.h>

@implementation iOSBridge

@end

extern "C"
{
    void _GetCurrentMediaTime()
    {
        struct timeval boottime;
        int mib[2] = {CTL_KERN, KERN_BOOTTIME};
        size_t size = sizeof(boottime);
        time_t now;
        time_t uptime = -1;
        
        (void)time(&now);
        
        if (sysctl(mib, 2, &boottime, &size, NULL, 0) != -1 && boottime.tv_sec != 0)
        {
            uptime = now - boottime.tv_sec;
        }

        float timeInFloat = uptime;
        NSString *str = [NSString stringWithFormat:@"%f",timeInFloat];
        const char *c = [str UTF8String];
        UnitySendMessage("EnergySuiteBehaviour", "CallbackGetTime", c);
    }
}